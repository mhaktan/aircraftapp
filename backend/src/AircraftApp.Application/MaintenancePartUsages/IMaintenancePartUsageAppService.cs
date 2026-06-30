using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AircraftApp.MaintenancePartUsages.Dto;

namespace AircraftApp.MaintenancePartUsages
{
    public interface IMaintenancePartUsageAppService : IAsyncCrudAppService<
        MaintenancePartUsageDto,
        long,
        PagedMaintenancePartUsageResultRequestDto,
        CreateMaintenancePartUsageDto,
        MaintenancePartUsageDto>
    {
    }
}
