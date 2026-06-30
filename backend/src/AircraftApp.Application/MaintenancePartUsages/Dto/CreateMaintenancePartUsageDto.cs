using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace AircraftApp.MaintenancePartUsages.Dto
{
    [AutoMapTo(typeof(Entities.MaintenancePartUsage))]
    public class CreateMaintenancePartUsageDto
    {
        public int QuantityUsed { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public long MaintenanceLogId { get; set; }

        public long SparePartId { get; set; }

    }
}