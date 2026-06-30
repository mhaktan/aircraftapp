using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using AircraftApp.SpareParts.Dto;

namespace AircraftApp.SpareParts
{
    public interface ISparePartAppService : IAsyncCrudAppService<
        SparePartDto,
        long,
        PagedSparePartResultRequestDto,
        CreateSparePartDto,
        SparePartDto>
    {
    }
}
