using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace AircraftApp.Entities
{
    // State Machine: status — Draft → PendingApproval → Approved → Rejected → Ordered → Received → Cancelled
    // Initial: Draft | Transitions: Draft→PendingApproval[Submit], PendingApproval→Approved[Approve], PendingApproval→Rejected[Reject], Approved→Ordered[PlaceOrder], Ordered→Received[ReceiveGoods], *→Cancelled[Cancel]
    [Table("PurchaseRequests")]
    public class PurchaseRequest : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(50)]
        public string RequestNumber { get; set; }

        public int RequestedQuantity { get; set; }

        public UrgencyLevel UrgencyLevel { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Justification { get; set; }

        public decimal? EstimatedUnitPrice { get; set; }

        [Required]
        [MaxLength(200)]
        public string RequestedBy { get; set; }

        public Status Status { get; set; }

        public long SparePartId { get; set; }

        [ForeignKey(nameof(SparePartId))]
        public virtual SparePart SparePart { get; set; }

    }
}