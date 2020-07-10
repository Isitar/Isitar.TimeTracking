namespace Isitar.TimeTracking.Application.Project.Queries.ProjectDetail
{
    using System;
    using MediatR;
    using User.Queries.UserDetail;

    public class ProjectDetailQuery : IRequest<ProjectDetailVm>
    {
        public Guid Id { get; set; }
    }
}