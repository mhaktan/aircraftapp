using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using AircraftApp.Entities;
using AircraftApp.MaintenancePartUsages.Dto;
using AircraftApp.Authorization;
using AircraftApp.Flows;

namespace AircraftApp.MaintenancePartUsages
{
    public class MaintenancePartUsageAppService : AsyncCrudAppService<
        MaintenancePartUsage,
        MaintenancePartUsageDto,
        long,
        PagedMaintenancePartUsageResultRequestDto,
        CreateMaintenancePartUsageDto,
        MaintenancePartUsageDto>,
        IMaintenancePartUsageAppService
    {
        private readonly IFlowEngine _flowEngine;

        public MaintenancePartUsageAppService(IRepository<MaintenancePartUsage, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.MaintenancePartUsage_Read;
            GetAllPermissionName = PermissionNames.MaintenancePartUsage_Read;
            CreatePermissionName = PermissionNames.MaintenancePartUsage_Create;
            UpdatePermissionName = PermissionNames.MaintenancePartUsage_Update;
            DeletePermissionName = PermissionNames.MaintenancePartUsage_Delete;
        }

        protected override IQueryable<MaintenancePartUsage> CreateFilteredQuery(PagedMaintenancePartUsageResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Notes != null && x.Notes.Contains(input.Keyword)))
                .WhereIf(!input.Notes.IsNullOrWhiteSpace(), x => x.Notes != null && x.Notes.Contains(input.Notes))
                .WhereIf(input.QuantityUsed.HasValue, x => x.QuantityUsed == input.QuantityUsed.Value)
                .WhereIf(input.MaintenanceLogId.HasValue, x => x.MaintenanceLogId == input.MaintenanceLogId.Value)
                .WhereIf(input.SparePartId.HasValue, x => x.SparePartId == input.SparePartId.Value);
        }

        public override async Task<MaintenancePartUsageDto> CreateAsync(CreateMaintenancePartUsageDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "MaintenancePartUsage", result);
            return result;
        }

        public override async Task<MaintenancePartUsageDto> UpdateAsync(MaintenancePartUsageDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "MaintenancePartUsage", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "MaintenancePartUsage", new { Id = input.Id });
        }
    }
}
