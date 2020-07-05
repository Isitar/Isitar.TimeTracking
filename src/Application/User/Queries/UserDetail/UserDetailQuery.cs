namespace Isitar.TimeTracking.Application.User.Queries.UserDetail
{
    using System;
    using MediatR;

    public class UserDetailQuery : IRequest<UserDetailVm>
    {
        public Guid Id { get; set; }
    }
}