namespace Isitar.TimeTracking.Application.User.Commands.CreateUserCommand
{
    using System;
    using MediatR;

    public class CreateUserCommand: IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }

        public string Locale { get; set; }

        public string Password { get; set; }
        public string Username { get; set; }
    }
}