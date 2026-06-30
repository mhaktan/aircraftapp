using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace AircraftApp.Entities
{
    [Table("SpareParts")]
    public class SparePart : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(100)]
        public string PartNumber { get; set; }

        [Required]
        [MaxLength(200)]
        public string PartName { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string UnitOfMeasure { get; set; }

        public int StockQuantity { get; set; }

        public int MinStockLevel { get; set; }

        public decimal? UnitPrice { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<MaintenancePartUsage> MaintenancePartUsages { get; set; }

        public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; }

    }
}