namespace Isitar.TimeTracking.Application.Common.Entities
{
    using System;
    using AutoMapper;
    using Domain.Entities;
    using Mappings;
    using NodaTime;

    public class AuditTrailDto : IMapFrom<AuditTrailEntry>
    {
        public Guid Id { get; set; }
        public Instant When { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public Guid CreatedById { get; set; }
        public string CreatedByName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AuditTrailEntry, AuditTrailDto>()
                .ForMember(dto => dto.CreatedByName, opts => opts.MapFrom(e => e.CreatedBy.Name));
        }
    }
}