using AutoMapper;
using AircraftApp.Entities;
using AircraftApp.MaintenanceRequests.Dto;

namespace AircraftApp.MaintenanceRequests
{
    public class MaintenanceRequestMapProfile : Profile
    {
        public MaintenanceRequestMapProfile()
        {
            CreateMap<MaintenanceRequest, MaintenanceRequestDto>()
                .ForMember(d => d.StatusName, o => o.MapFrom(s => s.Status.ToString()));
            CreateMap<CreateMaintenanceRequestDto, MaintenanceRequest>();
            CreateMap<MaintenanceRequestDto, MaintenanceRequest>();
        }
    }
}
