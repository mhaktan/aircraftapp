using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using AircraftApp.Entities;
using AircraftApp.MaintenanceTypes.Dto;
using AircraftApp.Authorization;
using AircraftApp.Flows;

namespace AircraftApp.MaintenanceTypes
{
    public class MaintenanceTypeAppService : AsyncCrudAppService<
        MaintenanceType,
        MaintenanceTypeDto,
        long,
        PagedMaintenanceTypeResultRequestDto,
        CreateMaintenanceTypeDto,
        MaintenanceTypeDto>,
        IMaintenanceTypeAppService
    {
        private readonly IFlowEngine _flowEngine;

        public MaintenanceTypeAppService(IRepository<MaintenanceType, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.MaintenanceType_Read;
            GetAllPermissionName = PermissionNames.MaintenanceType_Read;
            CreatePermissionName = PermissionNames.MaintenanceType_Create;
            UpdatePermissionName = PermissionNames.MaintenanceType_Update;
            DeletePermissionName = PermissionNames.MaintenanceType_Delete;
        }

        protected override IQueryable<MaintenanceType> CreateFilteredQuery(PagedMaintenanceTypeResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.Name != null && x.Name.Contains(input.Keyword)) ||
                    (x.Description != null && x.Description.Contains(input.Keyword)))
                .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name != null && x.Name.Contains(input.Name))
                .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description != null && x.Description.Contains(input.Description))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value);
        }

        public override async Task<MaintenanceTypeDto> CreateAsync(CreateMaintenanceTypeDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "MaintenanceType", result);
            return result;
        }

        public override async Task<MaintenanceTypeDto> UpdateAsync(MaintenanceTypeDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "MaintenanceType", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "MaintenanceType", new { Id = input.Id });
        }
    }
}
