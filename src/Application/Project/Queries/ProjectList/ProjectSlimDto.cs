namespace Isitar.TimeTracking.Application.Project.Queries.ProjectList
{
    using System;
    using Common.Mappings;
    using Domain.Entities;

    public class ProjectSlimDto : IMapFrom<Project>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
    }
}