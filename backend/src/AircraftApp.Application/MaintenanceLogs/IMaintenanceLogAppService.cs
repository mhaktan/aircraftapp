using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AircraftApp.MaintenanceLogs.Dto;

namespace AircraftApp.MaintenanceLogs
{
    public interface IMaintenanceLogAppService : IAsyncCrudAppService<
        MaintenanceLogDto,
        long,
        PagedMaintenanceLogResultRequestDto,
        CreateMaintenanceLogDto,
        MaintenanceLogDto>
    {
    }
}
