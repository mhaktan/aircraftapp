using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace AircraftApp.Entities
{
    [Table("MaintenanceLogs")]
    public class MaintenanceLog : FullAuditedEntity<long>
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

        [ForeignKey(nameof(MaintenanceRequestId))]
        public virtual MaintenanceRequest MaintenanceRequest { get; set; }

        public virtual ICollection<MaintenancePartUsage> MaintenancePartUsages { get; set; }

    }
}