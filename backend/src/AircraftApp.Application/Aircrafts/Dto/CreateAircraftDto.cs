using System;
using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace AircraftApp.Aircrafts.Dto
{
    [AutoMapTo(typeof(Entities.Aircraft))]
    public class CreateAircraftDto
    {
        [Required]
        [MaxLength(20)]
        public string RegistrationNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string AircraftType { get; set; }

        [Required]
        [MaxLength(100)]
        public string Manufacturer { get; set; }

        [Required]
        [MaxLength(100)]
        public string Model { get; set; }

        [Required]
        [MaxLength(50)]
        public string SerialNumber { get; set; }

        public bool IsActive { get; set; }

    }
}