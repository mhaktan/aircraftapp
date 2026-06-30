using AutoMapper;
using AircraftApp.Approvals.Dto;
using AircraftApp.Entities;

namespace AircraftApp.Approvals
{
    public class ApprovalMapProfile : Profile
    {
        public ApprovalMapProfile()
        {
            CreateMap<ApprovalRecord, ApprovalRecordDto>();
            CreateMap<StatusChangeLog, StatusChangeLogDto>();
        }
    }
}
