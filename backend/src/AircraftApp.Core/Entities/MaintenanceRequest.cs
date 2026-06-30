using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace AircraftApp.Entities
{
    // State Machine: status — Draft → PendingLineMechanicApproval → PendingQualityInspection → Approved → Revision → Completed → Cancelled
    // Initial: Draft | Transitions: Draft→PendingLineMechanicApproval[Submit], PendingLineMechanicApproval→PendingQualityInspection[Approve], PendingLineMechanicApproval→Revision[Revise], PendingQualityInspection→Approved[Approve], PendingQualityInspection→Revision[Revise], Revision→PendingLineMechanicApproval[Resubmit], Approved→Completed[Complete], *→Cancelled[Cancel]
    [Table("MaintenanceRequests")]
    public class MaintenanceRequest : FullAuditedEntity<long>
    {
        [Required]
        [MaxLength(50)]
        public string RequestNumber { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        public Priority Priority { get; set; }

        [Required]
        [MaxLength(200)]
        public string RequestedBy { get; set; }

        public long? LineMechanicId { get; set; }

        public long? QualityInspectorId { get; set; }

        public int? EstimatedDuration { get; set; }

        public int? ActualDuration { get; set; }

        [MaxLength(2000)]
        public string WorkPerformed { get; set; }

        [MaxLength(1000)]
        public string RevisionNote { get; set; }

        public Status Status { get; set; }

        public long AircraftId { get; set; }

        [ForeignKey(nameof(AircraftId))]
        public virtual Aircraft Aircraft { get; set; }

        public long MaintenanceTypeId { get; set; }

        [ForeignKey(nameof(MaintenanceTypeId))]
        public virtual MaintenanceType MaintenanceType { get; set; }

        public virtual ICollection<MaintenanceLog> MaintenanceLogs { get; set; }

    }
}