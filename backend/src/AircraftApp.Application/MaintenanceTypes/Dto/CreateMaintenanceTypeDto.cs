using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace AircraftApp.MaintenanceTypes.Dto
{
    [AutoMapTo(typeof(Entities.MaintenanceType))]
    public class CreateMaintenanceTypeDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

    }
}