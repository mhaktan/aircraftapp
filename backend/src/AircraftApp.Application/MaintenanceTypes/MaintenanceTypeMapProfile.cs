using AutoMapper;
using AircraftApp.Entities;
using AircraftApp.MaintenanceTypes.Dto;

namespace AircraftApp.MaintenanceTypes
{
    public class MaintenanceTypeMapProfile : Profile
    {
        public MaintenanceTypeMapProfile()
        {
            CreateMap<MaintenanceType, MaintenanceTypeDto>();
            CreateMap<CreateMaintenanceTypeDto, MaintenanceType>();
            CreateMap<MaintenanceTypeDto, MaintenanceType>();
        }
    }
}
