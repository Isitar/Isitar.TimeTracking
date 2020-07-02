namespace Isitar.TimeTracking.Application.TimeTracking.Commands.StopTrackingForUser
{
    using System;
    using MediatR;

    public class StopTrackingForUserCommand : IRequest
    {
        public Guid UserId { get; set; }
    }
}