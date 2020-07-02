namespace Isitar.TimeTracking.Application.Common.Interfaces
{
    using System;

    public interface ICurrentUserService
    {
        public Guid? UserId { get; }
        public bool IsAuthenticated { get; set; }
    }
}