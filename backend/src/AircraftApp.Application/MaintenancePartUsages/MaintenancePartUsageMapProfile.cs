using AutoMapper;
using AircraftApp.Entities;
using AircraftApp.MaintenancePartUsages.Dto;

namespace AircraftApp.MaintenancePartUsages
{
    public class MaintenancePartUsageMapProfile : Profile
    {
        public MaintenancePartUsageMapProfile()
        {
            CreateMap<MaintenancePartUsage, MaintenancePartUsageDto>();
            CreateMap<CreateMaintenancePartUsageDto, MaintenancePartUsage>();
            CreateMap<MaintenancePartUsageDto, MaintenancePartUsage>();
        }
    }
}
