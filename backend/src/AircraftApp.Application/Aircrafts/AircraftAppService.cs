using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using AircraftApp.Entities;
using AircraftApp.Aircrafts.Dto;
using AircraftApp.Authorization;
using AircraftApp.Flows;

namespace AircraftApp.Aircrafts
{
    public class AircraftAppService : AsyncCrudAppService<
        Aircraft,
        AircraftDto,
        long,
        PagedAircraftResultRequestDto,
        CreateAircraftDto,
        AircraftDto>,
        IAircraftAppService
    {
        private readonly IFlowEngine _flowEngine;

        public AircraftAppService(IRepository<Aircraft, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.Aircraft_Read;
            GetAllPermissionName = PermissionNames.Aircraft_Read;
            CreatePermissionName = PermissionNames.Aircraft_Create;
            UpdatePermissionName = PermissionNames.Aircraft_Update;
            DeletePermissionName = PermissionNames.Aircraft_Delete;
        }

        protected override IQueryable<Aircraft> CreateFilteredQuery(PagedAircraftResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.RegistrationNumber != null && x.RegistrationNumber.Contains(input.Keyword)) ||
                    (x.AircraftType != null && x.AircraftType.Contains(input.Keyword)) ||
                    (x.Manufacturer != null && x.Manufacturer.Contains(input.Keyword)) ||
                    (x.Model != null && x.Model.Contains(input.Keyword)) ||
                    (x.SerialNumber != null && x.SerialNumber.Contains(input.Keyword)))
                .WhereIf(!input.RegistrationNumber.IsNullOrWhiteSpace(), x => x.RegistrationNumber != null && x.RegistrationNumber.Contains(input.RegistrationNumber))
                .WhereIf(!input.AircraftType.IsNullOrWhiteSpace(), x => x.AircraftType != null && x.AircraftType.Contains(input.AircraftType))
                .WhereIf(!input.Manufacturer.IsNullOrWhiteSpace(), x => x.Manufacturer != null && x.Manufacturer.Contains(input.Manufacturer))
                .WhereIf(!input.Model.IsNullOrWhiteSpace(), x => x.Model != null && x.Model.Contains(input.Model))
                .WhereIf(!input.SerialNumber.IsNullOrWhiteSpace(), x => x.SerialNumber != null && x.SerialNumber.Contains(input.SerialNumber))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value);
        }

        public override async Task<AircraftDto> CreateAsync(CreateAircraftDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "Aircraft", result);
            return result;
        }

        public override async Task<AircraftDto> UpdateAsync(AircraftDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "Aircraft", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "Aircraft", new { Id = input.Id });
        }
    }
}
