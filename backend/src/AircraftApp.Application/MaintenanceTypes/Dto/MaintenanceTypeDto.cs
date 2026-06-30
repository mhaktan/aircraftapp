using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace AircraftApp.MaintenanceTypes.Dto
{
    [AutoMapFrom(typeof(Entities.MaintenanceType))]
    public class MaintenanceTypeDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}