namespace Isitar.TimeTracking.Application.User.Queries.UserDetail
{
    using System;
    using AutoMapper;
    using Common.Mappings;
    using Domain.Entities;
    using NodaTime;

    public class AuditTrailEntryDto : IMapFrom<AuditTrailEntry>
    {
        public Guid Id { get; set; }
        public Instant When { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public Guid? CreatedById { get; set; }
        public string CreatedByName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AuditTrailEntry, AuditTrailEntryDto>()
                .ForMember(a => a.CreatedByName,
                    opts => opts.MapFrom(a => a.CreatedBy.Name));
        }
    }
}