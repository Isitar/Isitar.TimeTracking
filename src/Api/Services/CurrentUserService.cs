namespace Api.Services
{
    using System;
    using System.Security.Claims;
    using Isitar.TimeTracking.Application.Common.Interfaces;
    using Microsoft.AspNetCore.Http;

    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var userIdString = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdString))
            {
                UserId = null;
            }
            else
            {
                UserId = Guid.Parse(userIdString);
            }
            IsAuthenticated = UserId != null;
        }

        public Guid? UserId { get; }
        public bool IsAuthenticated { get; set; }
    }
}