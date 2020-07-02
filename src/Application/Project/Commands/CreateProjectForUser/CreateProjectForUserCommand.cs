namespace Isitar.TimeTracking.Application.Project.Commands.CreateProject
{
    using System;
    using System.IO;
    using MediatR;

    public class CreateProjectForUserCommand : IRequest
    {
        public Guid UserId { get; set; }


        public Guid Id { get; set; }
        public string Name { get; set; }
        public Stream ImageStream { get; set; }
        public string ImageStreamFilename { get; set; }
    }
}