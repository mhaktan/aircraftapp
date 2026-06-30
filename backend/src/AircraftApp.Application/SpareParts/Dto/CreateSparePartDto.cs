using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace AircraftApp.SpareParts.Dto
{
    [AutoMapTo(typeof(Entities.SparePart))]
    public class CreateSparePartDto
    {
        [Required]
        [MaxLength(100)]
        public string PartNumber { get; set; }

        [Required]
        [MaxLength(200)]
        public string PartName { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string UnitOfMeasure { get; set; }

        public int StockQuantity { get; set; }

        public int MinStockLevel { get; set; }

        public decimal? UnitPrice { get; set; }

        public bool IsActive { get; set; }

    }
}