using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace AircraftApp.Entities
{
    [Table("Aircrafts")]
    public class Aircraft : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(20)]
        public string RegistrationNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string AircraftType { get; set; }

        [Required]
        [MaxLength(100)]
        public string Manufacturer { get; set; }

        [Required]
        [MaxLength(100)]
        public string Model { get; set; }

        [Required]
        [MaxLength(50)]
        public string SerialNumber { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<MaintenanceRequest> MaintenanceRequests { get; set; }

    }
}