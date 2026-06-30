using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace AircraftApp.Aircrafts.Dto
{
    [AutoMapFrom(typeof(Entities.Aircraft))]
    public class AircraftDto : EntityDto<long>
    {
        public string RegistrationNumber { get; set; }

        public string AircraftType { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public string SerialNumber { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}