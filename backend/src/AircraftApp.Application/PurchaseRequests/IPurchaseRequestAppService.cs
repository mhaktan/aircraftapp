using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AircraftApp.PurchaseRequests.Dto;

namespace AircraftApp.PurchaseRequests
{
    public interface IPurchaseRequestAppService : IAsyncCrudAppService<
        PurchaseRequestDto,
        long,
        PagedPurchaseRequestResultRequestDto,
        CreatePurchaseRequestDto,
        PurchaseRequestDto>
    {
        Task<PurchaseRequestDto> ChangeStatusAsync(long id, ChangeStatusInput input);
    }
}
