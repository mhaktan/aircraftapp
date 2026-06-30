using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AircraftApp.MaintenanceTypes.Dto;

namespace AircraftApp.MaintenanceTypes
{
    public interface IMaintenanceTypeAppService : IAsyncCrudAppService<
        MaintenanceTypeDto,
        long,
        PagedMaintenanceTypeResultRequestDto,
        CreateMaintenanceTypeDto,
        MaintenanceTypeDto>
    {
    }
}
