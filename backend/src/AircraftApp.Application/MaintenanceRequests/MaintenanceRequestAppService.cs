using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using AircraftApp.Entities;
using AircraftApp.MaintenanceRequests.Dto;
using AircraftApp.Authorization;
using AircraftApp.Flows;

namespace AircraftApp.MaintenanceRequests
{
    public class MaintenanceRequestAppService : AsyncCrudAppService<
        MaintenanceRequest,
        MaintenanceRequestDto,
        long,
        PagedMaintenanceRequestResultRequestDto,
        CreateMaintenanceRequestDto,
        MaintenanceRequestDto>,
        IMaintenanceRequestAppService
    {
        private readonly IRepository<StatusChangeLog, long> _statusChangeLogRepo;
        private readonly IRepository<ApprovalRecord, Guid> _approvalRepo;
        private readonly IFlowEngine _flowEngine;

        public MaintenanceRequestAppService(IRepository<MaintenanceRequest, long> repository, IFlowEngine flowEngine, IRepository<StatusChangeLog, long> statusChangeLogRepo, IRepository<ApprovalRecord, Guid> approvalRepo)
            : base(repository)
        {
            _flowEngine = flowEngine;
            _statusChangeLogRepo = statusChangeLogRepo;
            _approvalRepo = approvalRepo;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.MaintenanceRequest_Read;
            GetAllPermissionName = PermissionNames.MaintenanceRequest_Read;
            CreatePermissionName = PermissionNames.MaintenanceRequest_Create;
            UpdatePermissionName = PermissionNames.MaintenanceRequest_Update;
            DeletePermissionName = PermissionNames.MaintenanceRequest_Delete;
        }

        protected override IQueryable<MaintenanceRequest> CreateFilteredQuery(PagedMaintenanceRequestResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.RequestNumber != null && x.RequestNumber.Contains(input.Keyword)) ||
                    (x.Description != null && x.Description.Contains(input.Keyword)) ||
                    (x.RequestedBy != null && x.RequestedBy.Contains(input.Keyword)) ||
                    (x.WorkPerformed != null && x.WorkPerformed.Contains(input.Keyword)) ||
                    (x.RevisionNote != null && x.RevisionNote.Contains(input.Keyword)))
                .WhereIf(!input.RequestNumber.IsNullOrWhiteSpace(), x => x.RequestNumber != null && x.RequestNumber.Contains(input.RequestNumber))
                .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description != null && x.Description.Contains(input.Description))
                .WhereIf(!input.RequestedBy.IsNullOrWhiteSpace(), x => x.RequestedBy != null && x.RequestedBy.Contains(input.RequestedBy))
                .WhereIf(!input.WorkPerformed.IsNullOrWhiteSpace(), x => x.WorkPerformed != null && x.WorkPerformed.Contains(input.WorkPerformed))
                .WhereIf(!input.RevisionNote.IsNullOrWhiteSpace(), x => x.RevisionNote != null && x.RevisionNote.Contains(input.RevisionNote))
                .WhereIf(input.Priority.HasValue, x => x.Priority == (Priority)input.Priority.Value)
                .WhereIf(input.LineMechanicId.HasValue, x => x.LineMechanicId == input.LineMechanicId.Value)
                .WhereIf(input.QualityInspectorId.HasValue, x => x.QualityInspectorId == input.QualityInspectorId.Value)
                .WhereIf(input.EstimatedDuration.HasValue, x => x.EstimatedDuration == input.EstimatedDuration.Value)
                .WhereIf(input.ActualDuration.HasValue, x => x.ActualDuration == input.ActualDuration.Value)
                .WhereIf(input.Status.HasValue, x => x.Status == (Status)input.Status.Value)
                .WhereIf(input.AircraftId.HasValue, x => x.AircraftId == input.AircraftId.Value)
                .WhereIf(input.MaintenanceTypeId.HasValue, x => x.MaintenanceTypeId == input.MaintenanceTypeId.Value);
        }

        public override async Task<MaintenanceRequestDto> CreateAsync(CreateMaintenanceRequestDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "MaintenanceRequest", result);

            // Frontend creates records with status pre-set without going through ChangeStatusAsync,
            // so mirror on-field-change here whenever the initial status isn't the default. Otherwise
            // status-driven flows (e.g. approval) never fire on plain Create.
            if (result.Status != (int)Status.Draft)
                await _flowEngine.TriggerAsync("on-field-change", "MaintenanceRequest", result);
            return result;
        }

        public override async Task<MaintenanceRequestDto> UpdateAsync(MaintenanceRequestDto input)
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
                    EntityType = "MaintenanceRequest",
                    EntityId = input.Id.ToString(),
                    FromStatus = fromStatus,
                    ToStatus = toStatus,
                    Action = "Update",
                    ChangedByUserId = AbpSession.UserId
                });
            }

            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "MaintenanceRequest", result);

            // Frontend updates status via plain UpdateAsync (not ChangeStatusAsync) — fire
            // on-field-change so status-driven flows pick up the transition.
            if (statusChanged)
                await _flowEngine.TriggerAsync("on-field-change", "MaintenanceRequest", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "MaintenanceRequest", new { Id = input.Id });
        }

        [Abp.Authorization.AbpAuthorize(PermissionNames.MaintenanceRequest_Update)]
        public async Task<MaintenanceRequestDto> ChangeStatusAsync(long id, ChangeStatusInput input)
        {
            var entity = await Repository.GetAsync(id);
            var currentStatus = entity.Status.ToString();

            // Find valid transition
            var transitions = new (string From, string To, string Action, bool Readonly)[]
            {
            ("Draft", "PendingLineMechanicApproval", "Submit", false),
            ("PendingLineMechanicApproval", "PendingQualityInspection", "Approve", false),
            ("PendingLineMechanicApproval", "Revision", "Revise", false),
            ("PendingQualityInspection", "Approved", "Approve", false),
            ("PendingQualityInspection", "Revision", "Revise", false),
            ("Revision", "PendingLineMechanicApproval", "Resubmit", false),
            ("Approved", "Completed", "Complete", false),
            ("*", "Cancelled", "Cancel", true)
            };

            var transition = transitions.FirstOrDefault(t =>
                (t.From == "*" || t.From == currentStatus) && t.Action == input.Action);

            if (transition == default)
                throw new Abp.UI.UserFriendlyException($"Invalid action '{input.Action}' from status '{currentStatus}'");

            // Validate required fields per transition
            if (input.Action == "Revise" && (input.ActionData == null || !input.ActionData.ContainsKey("revisionNote") || string.IsNullOrWhiteSpace(input.ActionData["revisionNote"])))
                throw new Abp.UI.UserFriendlyException("Revise requires: revisionNote");

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
                    .Where(a => a.EntityType == "MaintenanceRequest" && a.EntityId == id.ToString() && a.Status == "Pending")
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
                EntityType = "MaintenanceRequest",
                EntityId = id.ToString(),
                FromStatus = fromStatus,
                ToStatus = transition.To,
                Action = input.Action,
                Comment = input.ActionData != null && input.ActionData.ContainsKey("comment") ? input.ActionData["comment"] : null,
                ChangedByUserId = AbpSession.UserId
            });

            var result = MapToEntityDto(entity);

            // Trigger flow: on-status-change (always)
            await _flowEngine.TriggerAsync("on-field-change", "MaintenanceRequest", result);

            // Trigger named flow events
            if (input.Action == "Submit")
                await _flowEngine.TriggerAsync("submit-for-approval", "MaintenanceRequest", result);
            return result;
        }

        private void ValidateStatusTransition(Status from, Status to)
        {
            var allowed = new (string From, string To)[]
            {
                ("Draft", "PendingLineMechanicApproval"),
                ("PendingLineMechanicApproval", "PendingQualityInspection"),
                ("PendingLineMechanicApproval", "Revision"),
                ("PendingQualityInspection", "Approved"),
                ("PendingQualityInspection", "Revision"),
                ("Revision", "PendingLineMechanicApproval"),
                ("Approved", "Completed"),
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
