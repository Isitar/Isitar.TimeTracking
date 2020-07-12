namespace Isitar.TimeTracking.Application.Project.Commands.ReplaceImage
{
    using System;
    using System.IO;
    using MediatR;

    public class ReplaceImageCommand : IRequest
    {
        public Guid Id { get; set; }
        public Stream ImageStream { get; set; }
        public string ImageStreamFilename { get; set; }
    }
}