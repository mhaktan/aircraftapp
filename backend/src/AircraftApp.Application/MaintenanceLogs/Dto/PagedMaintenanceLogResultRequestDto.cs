using System;
using Abp.Application.Services.Dto;

namespace AircraftApp.MaintenanceLogs.Dto
{
    public class PagedMaintenanceLogResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? MaintenanceRequestId { get; set; }
        public string PerformedBy { get; set; }
        public string WorkDescription { get; set; }
        public int? TimeSpent { get; set; }
        public DateTime? LogDate { get; set; }
    }
}
