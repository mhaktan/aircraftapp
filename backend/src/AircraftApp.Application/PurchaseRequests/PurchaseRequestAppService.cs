using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using AircraftApp.Entities;
using AircraftApp.PurchaseRequests.Dto;
using AircraftApp.Authorization;
using AircraftApp.Flows;

namespace AircraftApp.PurchaseRequests
{
    public class PurchaseRequestAppService : AsyncCrudAppService<
        PurchaseRequest,
        PurchaseRequestDto,
        long,
        PagedPurchaseRequestResultRequestDto,
        CreatePurchaseRequestDto,
        PurchaseRequestDto>,
        IPurchaseRequestAppService
    {
        private readonly IRepository<StatusChangeLog, long> _statusChangeLogRepo;
        private readonly IRepository<ApprovalRecord, Guid> _approvalRepo;
        private readonly IFlowEngine _flowEngine;

        public PurchaseRequestAppService(IRepository<PurchaseRequest, long> repository, IFlowEngine flowEngine, IRepository<StatusChangeLog, long> statusChangeLogRepo, IRepository<ApprovalRecord, Guid> approvalRepo)
            : base(repository)
        {
            _flowEngine = flowEngine;
            _statusChangeLogRepo = statusChangeLogRepo;
            _approvalRepo = approvalRepo;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.PurchaseRequest_Read;
            GetAllPermissionName = PermissionNames.PurchaseRequest_Read;
            CreatePermissionName = PermissionNames.PurchaseRequest_Create;
            UpdatePermissionName = PermissionNames.PurchaseRequest_Update;
            DeletePermissionName = PermissionNames.PurchaseRequest_Delete;
        }

        protected override IQueryable<PurchaseRequest> CreateFilteredQuery(PagedPurchaseRequestResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.RequestNumber != null && x.RequestNumber.Contains(input.Keyword)) ||
                    (x.Justification != null && x.Justification.Contains(input.Keyword)) ||
                    (x.RequestedBy != null && x.RequestedBy.Contains(input.Keyword)))
                .WhereIf(!input.RequestNumber.IsNullOrWhiteSpace(), x => x.RequestNumber != null && x.RequestNumber.Contains(input.RequestNumber))
                .WhereIf(!input.Justification.IsNullOrWhiteSpace(), x => x.Justification != null && x.Justification.Contains(input.Justification))
                .WhereIf(!input.RequestedBy.IsNullOrWhiteSpace(), x => x.RequestedBy != null && x.RequestedBy.Contains(input.RequestedBy))
                .WhereIf(input.RequestedQuantity.HasValue, x => x.RequestedQuantity == input.RequestedQuantity.Value)
                .WhereIf(input.UrgencyLevel.HasValue, x => x.UrgencyLevel == (UrgencyLevel)input.UrgencyLevel.Value)
                .WhereIf(input.EstimatedUnitPrice.HasValue, x => x.EstimatedUnitPrice == input.EstimatedUnitPrice.Value)
                .WhereIf(input.Status.HasValue, x => x.Status == (Status)input.Status.Value)
                .WhereIf(input.SparePartId.HasValue, x => x.SparePartId == input.SparePartId.Value);
        }

        public override async Task<PurchaseRequestDto> CreateAsync(CreatePurchaseRequestDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "PurchaseRequest", result);

            // Frontend creates records with status pre-set without going through ChangeStatusAsync,
            // so mirror on-field-change here whenever the initial status isn't the default. Otherwise
            // status-driven flows (e.g. approval) never fire on plain Create.
            if (result.Status != (int)Status.Draft)
                await _flowEngine.TriggerAsync("on-field-change", "PurchaseRequest", result);
            return result;
        }

        public override async Task<PurchaseRequestDto> UpdateAsync(PurchaseRequestDto input)
        {
            // State machine: validate status transition + log
            var existing = await Repository.GetAsync(input.Id);
            var statusChanged = (int)existing.Status != input.Status;
            if (statusChanged)
            {
                var fromStatus = existing.Status.ToString();
                var toStatus = ((Status)input.Status).ToString();
                ValidateStatusTransition(existing.Status, (Status)input.Status);

                // Log status change
                await _statusChangeLogRepo.InsertAsync(new StatusChangeLog
                {
                    EntityType = "PurchaseRequest",
                    EntityId = input.Id.ToString(),
                    FromStatus = fromStatus,
                    ToStatus = toStatus,
                    Action = "Update",
                    ChangedByUserId = AbpSession.UserId
                });
            }

            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "PurchaseRequest", result);

            // Frontend updates status via plain UpdateAsync (not ChangeStatusAsync) — fire
            // on-field-change so status-driven flows pick up the transition.
            if (statusChanged)
                await _flowEngine.TriggerAsync("on-field-change", "PurchaseRequest", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "PurchaseRequest", new { Id = input.Id });
        }

        [Abp.Authorization.AbpAuthorize(PermissionNames.PurchaseRequest_Update)]
        public async Task<PurchaseRequestDto> ChangeStatusAsync(long id, ChangeStatusInput input)
        {
            var entity = await Repository.GetAsync(id);
            var currentStatus = entity.Status.ToString();

            // Find valid transition
            var transitions = new (string From, string To, string Action, bool Readonly)[]
            {
            ("Draft", "PendingApproval", "Submit", false),
            ("PendingApproval", "Approved", "Approve", false),
            ("PendingApproval", "Rejected", "Reject", false),
            ("Approved", "Ordered", "PlaceOrder", false),
            ("Ordered", "Received", "ReceiveGoods", false),
            ("*", "Cancelled", "Cancel", true)
            };

            var transition = transitions.FirstOrDefault(t =>
                (t.From == "*" || t.From == currentStatus) && t.Action == input.Action);

            if (transition == default)
                throw new Abp.UI.UserFriendlyException($"Invalid action '{input.Action}' from status '{currentStatus}'");

            // Validate required fields per transition
            // No required fields for any transition

            var fromStatus = currentStatus;

            // Apply new status
            entity.Status = (Status)Enum.Parse(typeof(Status), transition.To);
            await Repository.UpdateAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            // Cancel pending ApprovalRecords when the entity is cancelled — otherwise the records
            // sit forever in approvers' inboxes pointing to a cancelled request.
            if (input.Action == "Cancel")
            {
                var pending = _approvalRepo.GetAll()
                    .Where(a => a.EntityType == "PurchaseRequest" && a.EntityId == id.ToString() && a.Status == "Pending")
                    .ToList();
                foreach (var pendingRec in pending)
                {
                    pendingRec.Status = "Cancelled";
                    pendingRec.ActionTaken = "Cancel";
                    pendingRec.ActionDate = DateTime.UtcNow;
                    pendingRec.Comment = "Entity cancelled by submitter.";
                    await _approvalRepo.UpdateAsync(pendingRec);
                }
            }

            // Log status change
            await _statusChangeLogRepo.InsertAsync(new Entities.StatusChangeLog
            {
                EntityType = "PurchaseRequest",
                EntityId = id.ToString(),
                FromStatus = fromStatus,
                ToStatus = transition.To,
                Action = input.Action,
                Comment = input.ActionData != null && input.ActionData.ContainsKey("comment") ? input.ActionData["comment"] : null,
                ChangedByUserId = AbpSession.UserId
            });

            var result = MapToEntityDto(entity);

            // Trigger flow: on-status-change (always)
            await _flowEngine.TriggerAsync("on-field-change", "PurchaseRequest", result);

            // Trigger named flow events
            if (input.Action == "Submit")
                await _flowEngine.TriggerAsync("submit-for-approval", "PurchaseRequest", result);
            return result;
        }

        private void ValidateStatusTransition(Status from, Status to)
        {
            var allowed = new (string From, string To)[]
            {
                ("Draft", "PendingApproval"),
                ("PendingApproval", "Approved"),
                ("PendingApproval", "Rejected"),
                ("Approved", "Ordered"),
                ("Ordered", "Received"),
                ("*", "Cancelled")
            };

            var isValid = allowed.Any(t =>
                (t.From == "*" || t.From == from.ToString()) &&
                t.To == to.ToString());

            if (!isValid)
                throw new Abp.UI.UserFriendlyException($"Invalid status transition from {from} to {to}");
        }
    }
}
