namespace Isitar.TimeTracking.Frontend.Common.Authentication
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components.Authorization;

    public static class AuthenticationStateProviderExtensions
    {
        public static async Task<Guid> UserIdAsync(this AuthenticationStateProvider authenticationStateProvider)
        {
            var authenticationState = await authenticationStateProvider.GetAuthenticationStateAsync();

            return authenticationState.User.Claims
                .Where(c => c.Type.Equals(ClaimTypes.NameIdentifier))
                .Select(c =>
                {
                    Guid x;
                    return Guid.TryParse(c.Value, out x) ? x : Guid.Empty;
                })
                .FirstOrDefault(guid => !Guid.Empty.Equals(guid));
        }
    }
}