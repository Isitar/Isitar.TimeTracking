namespace Isitar.TimeTracking.Infrastructure.Identity.Services.TokenService
{
    using System;
    using System.Threading.Tasks;
    using Application.Common.Entities;

    public interface ITokenService
    {
        /// <summary>
        ///     Generates a new AuthResponse with the given refreshToken and jwtToken
        /// </summary>
        /// <param name="refreshTokenString">the refresh token</param>
        /// <param name="jwtToken">the jwt token</param>
        /// <returns>A new AuthResponse containing either the tokens or error messages</returns>
        Task<AuthResponse> RefreshAsync(string refreshTokenString, string jwtToken);

        Task<AuthResponse> LoginAsync(string username, string password);
        Task<Result> LogoutAsync(Guid userId);
    }
}