namespace Isitar.TimeTracking.Api.Services
{
    using System;
    using System.Security.Claims;
    using Application.Common.Interfaces;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;

    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var userIdString = httpContextAccessor.HttpContext?.User?.FindFirstValue(new IdentityOptions().ClaimsIdentity.UserIdClaimType);
            var asdf = httpContextAccessor.HttpContext?.User?.FindFirstValue(new IdentityOptions().ClaimsIdentity.UserNameClaimType);
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