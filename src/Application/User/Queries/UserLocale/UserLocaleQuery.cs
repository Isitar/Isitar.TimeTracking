namespace Isitar.TimeTracking.Application.User.Queries.UserLocale
{
    using System;
    using MediatR;

    public class UserLocaleQuery : IRequest<UserLocaleVm>
    {
        public Guid Id { get; set; }
    }
}