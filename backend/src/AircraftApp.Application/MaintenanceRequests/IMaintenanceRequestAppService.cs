using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AircraftApp.MaintenanceRequests.Dto;

namespace AircraftApp.MaintenanceRequests
{
    public interface IMaintenanceRequestAppService : IAsyncCrudAppService<
        MaintenanceRequestDto,
        long,
        PagedMaintenanceRequestResultRequestDto,
        CreateMaintenanceRequestDto,
        MaintenanceRequestDto>
    {
        Task<MaintenanceRequestDto> ChangeStatusAsync(long id, ChangeStatusInput input);
    }
}
