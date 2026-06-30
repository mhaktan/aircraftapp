using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace AircraftApp.SpareParts.Dto
{
    [AutoMapFrom(typeof(Entities.SparePart))]
    public class SparePartDto : EntityDto<long>
    {
        public string PartNumber { get; set; }

        public string PartName { get; set; }

        public string Description { get; set; }

        public string UnitOfMeasure { get; set; }

        public int StockQuantity { get; set; }

        public int MinStockLevel { get; set; }

        public decimal? UnitPrice { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}