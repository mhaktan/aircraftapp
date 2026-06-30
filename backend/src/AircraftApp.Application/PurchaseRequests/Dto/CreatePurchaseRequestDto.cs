using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace AircraftApp.PurchaseRequests.Dto
{
    [AutoMapTo(typeof(Entities.PurchaseRequest))]
    public class CreatePurchaseRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string RequestNumber { get; set; }

        public int RequestedQuantity { get; set; }

        public int UrgencyLevel { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Justification { get; set; }

        public decimal? EstimatedUnitPrice { get; set; }

        [Required]
        [MaxLength(200)]
        public string RequestedBy { get; set; }

        public int Status { get; set; }

        public long SparePartId { get; set; }

    }
}