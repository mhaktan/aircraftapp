using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace AircraftApp.MaintenancePartUsages.Dto
{
    [AutoMapFrom(typeof(Entities.MaintenancePartUsage))]
    public class MaintenancePartUsageDto : EntityDto<long>
    {
        public int QuantityUsed { get; set; }

        public string Notes { get; set; }

        public long MaintenanceLogId { get; set; }

        public long SparePartId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}