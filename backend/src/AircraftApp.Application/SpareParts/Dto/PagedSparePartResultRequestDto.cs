using System;
using Abp.Application.Services.Dto;

namespace AircraftApp.SpareParts.Dto
{
    public class PagedSparePartResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }
        public int? StockQuantity { get; set; }
        public int? MinStockLevel { get; set; }
        public decimal? UnitPrice { get; set; }
        public bool? IsActive { get; set; }
    }
}
