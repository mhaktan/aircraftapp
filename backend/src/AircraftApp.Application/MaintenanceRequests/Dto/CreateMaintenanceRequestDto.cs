using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace AircraftApp.MaintenanceRequests.Dto
{
    [AutoMapTo(typeof(Entities.MaintenanceRequest))]
    public class CreateMaintenanceRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string RequestNumber { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }

        public int Priority { get; set; }

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

        public int Status { get; set; }

        public long AircraftId { get; set; }

        public long MaintenanceTypeId { get; set; }

    }
}