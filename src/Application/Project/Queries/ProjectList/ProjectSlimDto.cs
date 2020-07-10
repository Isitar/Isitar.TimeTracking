namespace Isitar.TimeTracking.Application.Project.Queries.ProjectList
{
    using System;
    using AutoMapper;
    using Common.Mappings;
    using Domain.Entities;

    public class ProjectSlimDto : IMapFrom<Project>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool HasImage { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Project, ProjectSlimDto>()
                .ForMember(vm => vm.HasImage, opt => opt.MapFrom(p => !string.IsNullOrWhiteSpace(p.ImagePath)))
                ;
        }
    }
}