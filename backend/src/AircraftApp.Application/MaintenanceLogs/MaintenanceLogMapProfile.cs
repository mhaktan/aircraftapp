using AutoMapper;
using AircraftApp.Entities;
using AircraftApp.MaintenanceLogs.Dto;

namespace AircraftApp.MaintenanceLogs
{
    public class MaintenanceLogMapProfile : Profile
    {
        public MaintenanceLogMapProfile()
        {
            CreateMap<MaintenanceLog, MaintenanceLogDto>();
            CreateMap<CreateMaintenanceLogDto, MaintenanceLog>();
            CreateMap<MaintenanceLogDto, MaintenanceLog>();
        }
    }
}
