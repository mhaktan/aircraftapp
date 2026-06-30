using AutoMapper;
using AircraftApp.Entities;
using AircraftApp.Aircrafts.Dto;

namespace AircraftApp.Aircrafts
{
    public class AircraftMapProfile : Profile
    {
        public AircraftMapProfile()
        {
            CreateMap<Aircraft, AircraftDto>();
            CreateMap<CreateAircraftDto, Aircraft>();
            CreateMap<AircraftDto, Aircraft>();
        }
    }
}
