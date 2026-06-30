using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace AircraftApp.MaintenanceRequests.Dto
{
    [AutoMapFrom(typeof(Entities.MaintenanceRequest))]
    public class MaintenanceRequestDto : EntityDto<long>
    {
        public string RequestNumber { get; set; }

        public string Description { get; set; }

        public int Priority { get; set; }

        public string RequestedBy { get; set; }

        public long? LineMechanicId { get; set; }

        public long? QualityInspectorId { get; set; }

        public int? EstimatedDuration { get; set; }

        public int? ActualDuration { get; set; }

        public string WorkPerformed { get; set; }

        public string RevisionNote { get; set; }

        public int Status { get; set; }

        /// <summary>
        /// String form of the status — used by flow conditions (triggerData.statusName equals "PendingX").
        /// </summary>
        public string StatusName { get; set; }

        public long AircraftId { get; set; }

        public long MaintenanceTypeId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}