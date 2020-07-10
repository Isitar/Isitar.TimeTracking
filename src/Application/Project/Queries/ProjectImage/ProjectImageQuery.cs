namespace Isitar.TimeTracking.Application.Project.Queries.ProjectImage
{
    using System;
    using MediatR;

    public class ProjectImageQuery : IRequest<ProjectImageDto>
    {
        public Guid Id { get; set; }
    }
}