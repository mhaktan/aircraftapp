using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace AircraftApp.MaintenanceLogs.Dto
{
    [AutoMapTo(typeof(Entities.MaintenanceLog))]
    public class CreateMaintenanceLogDto
    {
        [Required]
        [MaxLength(200)]
        public string PerformedBy { get; set; }

        [Required]
        [MaxLength(2000)]
        public string WorkDescription { get; set; }

        public int TimeSpent { get; set; }

        public DateTime LogDate { get; set; }

        public long MaintenanceRequestId { get; set; }

    }
}