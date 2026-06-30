using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace AircraftApp.Entities
{
    [Table("MaintenancePartUsages")]
    public class MaintenancePartUsage : FullAuditedEntity<long>
    {
        public int QuantityUsed { get; set; }

        [MaxLength(500)]
        public string Notes { get; set; }

        public long MaintenanceLogId { get; set; }

        [ForeignKey(nameof(MaintenanceLogId))]
        public virtual MaintenanceLog MaintenanceLog { get; set; }

        public long SparePartId { get; set; }

        [ForeignKey(nameof(SparePartId))]
        public virtual SparePart SparePart { get; set; }

    }
}