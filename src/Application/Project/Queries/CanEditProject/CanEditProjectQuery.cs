namespace Isitar.TimeTracking.Application.Project.Queries.CanEditProject
{
    using System;
    using MediatR;

    public class CanEditProjectQuery : IRequest<bool>
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
    }
}