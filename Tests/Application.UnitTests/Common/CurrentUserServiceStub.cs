namespace Application.UnitTests.Common
{
    using System;
    using Isitar.TimeTracking.Application.Common.Interfaces;

    public class CurrentUserServiceStub : ICurrentUserService
    {
        public Guid? UserId { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}