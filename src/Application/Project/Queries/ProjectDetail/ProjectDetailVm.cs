namespace Isitar.TimeTracking.Application.Project.Queries.ProjectDetail
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Common.Mappings;
    using Domain.Entities;
    using NodaTime;
    using User.Queries.UserDetail;

    public class ProjectDetailVm : IMapFrom<Project>
    {
        public Guid UserId { get; set; }
        
        public string Name { get; set; }
        public bool HasImage { get; set; }

        public string CreatedByName { get; set; }
        public Instant? CreatedAt { get; set; }
        public string UpdatedByName { get; set; }
        public Instant? UpdatedAt { get; set; }

        public ICollection<AuditTrailEntry> AuditTrailEntries { get; protected set; } = new HashSet<AuditTrailEntry>();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Project, ProjectDetailVm>()
                .ForMember(vm => vm.CreatedByName, opt => opt.MapFrom(p => p.CreatedBy.Name))
                .ForMember(vm => vm.UpdatedByName, opt => opt.MapFrom(p => p.UpdatedBy.Name))
                .ForMember(vm => vm.AuditTrailEntries, opt => opt.MapFrom(p => p.AuditTrailEntries))
                .ForMember(vm => vm.HasImage, opt => opt.MapFrom(p => !string.IsNullOrWhiteSpace(p.ImagePath)))
                ;
        }
    }
}