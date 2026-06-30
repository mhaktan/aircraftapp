using System;
using Abp.Application.Services.Dto;

namespace AircraftApp.MaintenanceTypes.Dto
{
    public class PagedMaintenanceTypeResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
