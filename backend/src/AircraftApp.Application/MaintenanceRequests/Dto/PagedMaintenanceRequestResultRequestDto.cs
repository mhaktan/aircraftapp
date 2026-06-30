using System;
using Abp.Application.Services.Dto;

namespace AircraftApp.MaintenanceRequests.Dto
{
    public class PagedMaintenanceRequestResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? AircraftId { get; set; }
        public long? MaintenanceTypeId { get; set; }
        public string RequestNumber { get; set; }
        public string Description { get; set; }
        public int? Priority { get; set; }
        public string RequestedBy { get; set; }
        public long? LineMechanicId { get; set; }
        public long? QualityInspectorId { get; set; }
        public int? EstimatedDuration { get; set; }
        public int? ActualDuration { get; set; }
        public string WorkPerformed { get; set; }
        public string RevisionNote { get; set; }
        public int? Status { get; set; }
    }
}
