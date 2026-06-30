using AutoMapper;
using AircraftApp.Entities;
using AircraftApp.SpareParts.Dto;

namespace AircraftApp.SpareParts
{
    public class SparePartMapProfile : Profile
    {
        public SparePartMapProfile()
        {
            CreateMap<SparePart, SparePartDto>();
            CreateMap<CreateSparePartDto, SparePart>();
            CreateMap<SparePartDto, SparePart>();
        }
    }
}
