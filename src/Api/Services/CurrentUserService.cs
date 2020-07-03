namespace Isitar.TimeTracking.Api.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using Application.Common.Interfaces;
    using Microsoft.AspNetCore.Http;

    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var userIdString = httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
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