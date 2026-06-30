using System;
using Abp.Application.Services.Dto;

namespace AircraftApp.MaintenancePartUsages.Dto
{
    public class PagedMaintenancePartUsageResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? MaintenanceLogId { get; set; }
        public long? SparePartId { get; set; }
        public int? QuantityUsed { get; set; }
        public string Notes { get; set; }
    }
}
