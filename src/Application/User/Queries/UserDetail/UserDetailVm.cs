namespace Isitar.TimeTracking.Application.User.Queries.UserDetail
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Common.Mappings;
    using Domain.Entities;
    using NodaTime;

    public class UserDetailVm : IMapFrom<User>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public string Locale { get; set; }

        public string CreatedByName { get; set; }
        public Instant? CreatedAt { get; set; }
        public string UpdatedByName { get; set; }
        public Instant? UpdatedAt { get; set; }

        public ICollection<AuditTrailEntry> AuditTrailEntries { get; } = new HashSet<AuditTrailEntry>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDetailVm>()
                .ForMember(vm => vm.CreatedByName, opt => opt.MapFrom(u => u.CreatedBy.Name))
                .ForMember(vm => vm.UpdatedByName, opt => opt.MapFrom(u => u.UpdatedBy.Name));
        }
    }
}