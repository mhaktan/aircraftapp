using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AircraftApp.Aircrafts.Dto;

namespace AircraftApp.Aircrafts
{
    public interface IAircraftAppService : IAsyncCrudAppService<
        AircraftDto,
        long,
        PagedAircraftResultRequestDto,
        CreateAircraftDto,
        AircraftDto>
    {
    }
}
