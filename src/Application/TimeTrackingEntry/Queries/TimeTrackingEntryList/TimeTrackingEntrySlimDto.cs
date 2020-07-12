namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingList
{
    using System;
    using AutoMapper;
    using Common.Mappings;
    using Domain.Entities;
    using NodaTime;
    using TimeTrackingEntryDetail;

    public class TimeTrackingEntrySlimDto : IMapFrom<TimeTrackingEntry>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }

        public Instant From { get; set; }
        public Instant? To { get; set; }

        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TimeTrackingEntry, TimeTrackingEntrySlimDto>()
                .ForMember(vm => vm.UserName, opts => opts.MapFrom(e => e.User.Name))
                .ForMember(vm => vm.ProjectName, opts => opts.MapFrom(e => e.Project.Name))
                ;
        }
    }
}