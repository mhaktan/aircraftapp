using System;
using Abp.Application.Services.Dto;

namespace AircraftApp.PurchaseRequests.Dto
{
    public class PagedPurchaseRequestResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? SparePartId { get; set; }
        public string RequestNumber { get; set; }
        public int? RequestedQuantity { get; set; }
        public int? UrgencyLevel { get; set; }
        public string Justification { get; set; }
        public decimal? EstimatedUnitPrice { get; set; }
        public string RequestedBy { get; set; }
        public int? Status { get; set; }
    }
}
