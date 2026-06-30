using System;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace AircraftApp.MaintenanceLogs.Dto
{
    [AutoMapFrom(typeof(Entities.MaintenanceLog))]
    public class MaintenanceLogDto : EntityDto<long>
    {
        public string PerformedBy { get; set; }

        public string WorkDescription { get; set; }

        public int TimeSpent { get; set; }

        public DateTime LogDate { get; set; }

        public long MaintenanceRequestId { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}