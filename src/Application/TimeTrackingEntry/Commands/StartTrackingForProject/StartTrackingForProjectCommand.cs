namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Commands.StartTrackingForProject
{
    using System;
    using Domain.Entities;
    using MediatR;

    public class StartTrackingForProjectCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
    }
}