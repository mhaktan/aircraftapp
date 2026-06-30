using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace AircraftApp.PurchaseRequests.Dto
{
    [AutoMapFrom(typeof(Entities.PurchaseRequest))]
    public class PurchaseRequestDto : EntityDto<long>
    {
        public string RequestNumber { get; set; }

        public int RequestedQuantity { get; set; }

        public int UrgencyLevel { get; set; }

        public string Justification { get; set; }

        public decimal? EstimatedUnitPrice { get; set; }

        public string RequestedBy { get; set; }

        public int Status { get; set; }

        /// <summary>
        /// String form of the status — used by flow conditions (triggerData.statusName equals "PendingX").
        /// </summary>
        public string StatusName { get; set; }

        public long SparePartId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}