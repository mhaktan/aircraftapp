using System;
using Abp.Application.Services.Dto;

namespace AircraftApp.Aircrafts.Dto
{
    public class PagedAircraftResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public string RegistrationNumber { get; set; }
        public string AircraftType { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public bool? IsActive { get; set; }
    }
}
