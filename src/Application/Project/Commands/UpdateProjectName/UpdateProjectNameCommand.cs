namespace Isitar.TimeTracking.Application.Project.Commands.UpdateProjectName
{
    using System;
    using MediatR;

    public class UpdateProjectNameCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}