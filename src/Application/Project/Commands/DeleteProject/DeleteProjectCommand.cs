namespace Isitar.TimeTracking.Application.Project.Commands.DeleteProject
{
    using System;
    using MediatR;

    public class DeleteProjectCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}