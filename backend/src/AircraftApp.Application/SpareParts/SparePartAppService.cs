using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using AircraftApp.Entities;
using AircraftApp.SpareParts.Dto;
using AircraftApp.Authorization;
using AircraftApp.Flows;

namespace AircraftApp.SpareParts
{
    public class SparePartAppService : AsyncCrudAppService<
        SparePart,
        SparePartDto,
        long,
        PagedSparePartResultRequestDto,
        CreateSparePartDto,
        SparePartDto>,
        ISparePartAppService
    {
        private readonly IFlowEngine _flowEngine;

        public SparePartAppService(IRepository<SparePart, long> repository, IFlowEngine flowEngine)
            : base(repository)
        {
            _flowEngine = flowEngine;
            // Claim-based authorization (JwtPermissionChecker reads JWT "permission" claims)
            GetPermissionName = PermissionNames.SparePart_Read;
            GetAllPermissionName = PermissionNames.SparePart_Read;
            CreatePermissionName = PermissionNames.SparePart_Create;
            UpdatePermissionName = PermissionNames.SparePart_Update;
            DeletePermissionName = PermissionNames.SparePart_Delete;
        }

        protected override IQueryable<SparePart> CreateFilteredQuery(PagedSparePartResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x =>
                    x.Id.ToString().Contains(input.Keyword) ||
                    (x.PartNumber != null && x.PartNumber.Contains(input.Keyword)) ||
                    (x.PartName != null && x.PartName.Contains(input.Keyword)) ||
                    (x.Description != null && x.Description.Contains(input.Keyword)) ||
                    (x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(input.Keyword)))
                .WhereIf(!input.PartNumber.IsNullOrWhiteSpace(), x => x.PartNumber != null && x.PartNumber.Contains(input.PartNumber))
                .WhereIf(!input.PartName.IsNullOrWhiteSpace(), x => x.PartName != null && x.PartName.Contains(input.PartName))
                .WhereIf(!input.Description.IsNullOrWhiteSpace(), x => x.Description != null && x.Description.Contains(input.Description))
                .WhereIf(!input.UnitOfMeasure.IsNullOrWhiteSpace(), x => x.UnitOfMeasure != null && x.UnitOfMeasure.Contains(input.UnitOfMeasure))
                .WhereIf(input.StockQuantity.HasValue, x => x.StockQuantity == input.StockQuantity.Value)
                .WhereIf(input.MinStockLevel.HasValue, x => x.MinStockLevel == input.MinStockLevel.Value)
                .WhereIf(input.UnitPrice.HasValue, x => x.UnitPrice == input.UnitPrice.Value)
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value);
        }

        public override async Task<SparePartDto> CreateAsync(CreateSparePartDto input)
        {
            var result = await base.CreateAsync(input);
            await _flowEngine.TriggerAsync("on-create", "SparePart", result);
            return result;
        }

        public override async Task<SparePartDto> UpdateAsync(SparePartDto input)
        {
            var result = await base.UpdateAsync(input);
            await _flowEngine.TriggerAsync("on-update", "SparePart", result);
            return result;
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            await base.DeleteAsync(input);
            await _flowEngine.TriggerAsync("on-delete", "SparePart", new { Id = input.Id });
        }
    }
}
