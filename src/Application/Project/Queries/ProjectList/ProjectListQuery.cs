namespace Isitar.TimeTracking.Application.Project.Queries.ProjectList
{
    using System;
    using System.Collections.Generic;
    using MediatR;

    public class ProjectListQuery : IRequest<ProjectListVm>
    {
        public ICollection<Guid> UserFilter { get; set; } = new HashSet<Guid>();
    }
}