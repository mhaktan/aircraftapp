using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using AircraftApp.Entities;
using AircraftApp.MaintenanceLogs.Dto;
using AircraftApp.Authorization;
using AircraftApp.Flows;

namespace AircraftApp.MaintenanceLogs
{
    public class MaintenanceLogAppService : AsyncCrudAppService<
        MaintenanceLog,
        MaintenanceLogDto,
        long,
        PagedMaintenanceLogResultRequestDto,
        CreateMaintenanceLogDto,
        MaintenanceLogDto>,
        IMaintenanceLogAppService
    {
        private readonly IFlowEngine _flowEngine;

        public MaintenanceLogAppService(IRepository<MaintenanceLog, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.MaintenanceLog_Read;
            GetAllPermissionName = PermissionNames.MaintenanceLog_Read;
            CreatePermissionName = PermissionNames.MaintenanceLog_Create;
            UpdatePermissionName = PermissionNames.MaintenanceLog_Update;
            DeletePermissionName = PermissionNames.MaintenanceLog_Delete;
        }

        protected override IQueryable<MaintenanceLog> CreateFilteredQuery(PagedMaintenanceLogResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.PerformedBy != null && x.PerformedBy.Contains(input.Keyword)) ||
                    (x.WorkDescription != null && x.WorkDescription.Contains(input.Keyword)))
                .WhereIf(!input.PerformedBy.IsNullOrWhiteSpace(), x => x.PerformedBy != null && x.PerformedBy.Contains(input.PerformedBy))
                .WhereIf(!input.WorkDescription.IsNullOrWhiteSpace(), x => x.WorkDescription != null && x.WorkDescription.Contains(input.WorkDescription))
                .WhereIf(input.TimeSpent.HasValue, x => x.TimeSpent == input.TimeSpent.Value)
                .WhereIf(input.LogDate.HasValue, x => x.LogDate == input.LogDate.Value)
                .WhereIf(input.MaintenanceRequestId.HasValue, x => x.MaintenanceRequestId == input.MaintenanceRequestId.Value);
        }

        public override async Task<MaintenanceLogDto> CreateAsync(CreateMaintenanceLogDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "MaintenanceLog", result);
            return result;
        }

        public override async Task<MaintenanceLogDto> UpdateAsync(MaintenanceLogDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "MaintenanceLog", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "MaintenanceLog", new { Id = input.Id });
        }
    }
}
