namespace Isitar.TimeTracking.Infrastructure.Identity.Services.TokenService
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Application.Common.Entities;
    using Common;
    using Common.Resources;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using NodaTime;

    public class TokenService : ITokenService
    {
        private readonly AppIdentityDbContext identityDbContext;
        private readonly JwtSettings jwtSettings;
        private readonly TokenValidationParameters tokenValidationParameters;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly IInstant instant;

        public TokenService(AppIdentityDbContext identityDbContext,
            JwtSettings jwtSettings,
            TokenValidationParameters tokenValidationParameters,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IInstant instant
        )
        {
            this.identityDbContext = identityDbContext;
            this.jwtSettings = jwtSettings;
            this.tokenValidationParameters = tokenValidationParameters;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.instant = instant;
        }

        /// <summary>
        ///     Generates a new AuthResponse with the given refreshToken and jwtToken
        /// </summary>
        /// <param name="refreshTokenString">the refresh token</param>
        /// <param name="jwtToken">the jwt token</param>
        /// <returns>A new AuthResponse containing either the tokens or error messages</returns>
        public async Task<AuthResponse> RefreshAsync(string refreshTokenString, string jwtToken)
        {
            var errorResponse = AuthResponse.Failure(new[] {Translation.InvalidToken});

            var validatedToken = PrincipalFromToken(jwtToken);
            if (null == validatedToken)
            {
                return errorResponse;
            }


            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var identityOptions = new IdentityOptions();
            var storedRefreshToken = await identityDbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshTokenString);
            var userId = Guid.Parse(validatedToken.Claims.Single(x => x.Type == identityOptions.ClaimsIdentity.UserIdClaimType).Value);

            if (storedRefreshToken == null
                || instant.Now > storedRefreshToken.Expires
                || storedRefreshToken.Invalidated
                || storedRefreshToken.Used
                || storedRefreshToken.JwtTokenId != jti
                || userId != storedRefreshToken.UserId
            )
            {
                return errorResponse;
            }

            storedRefreshToken.Used = true;
            identityDbContext.RefreshTokens.Update(storedRefreshToken);
            await identityDbContext.SaveChangesAsync();

            var user = await userManager.FindByIdAsync(userId.ToString());
            return await GenerateAuthenticationResultForUserAsync(user);
        }

        /// <summary>
        ///     Generates a AuthResponse based on existing login data
        /// </summary>
        /// <param name="username">the username</param>
        /// <param name="password">the unhashed password</param>
        /// <returns>An AuthResponse with either the token inside or an error message</returns>
        public async Task<AuthResponse> LoginAsync(string username, string password)
        {
            var errorResponse = AuthResponse.Failure(new[] {Translation.UsernamePasswordWrong});

            var user = await userManager.FindByNameAsync(username) ?? await userManager.FindByEmailAsync(username);
            if (null == user || !await userManager.CheckPasswordAsync(user, password))
            {
                return errorResponse;
            }

            return await GenerateAuthenticationResultForUserAsync(user);
        }


        public async Task<Result> LogoutAsync(Guid userId)
        {
            var user = await userManager.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Id.Equals(userId));
            if (null == user)
            {
                return Result.Failure(new[] {Translation.NotFoundException.Replace("{name}", Translation.User).Replace("{key}", userId.ToString())});
            }

            foreach (var userRefreshToken in user.RefreshTokens.Where(rt => !(rt.Invalidated || rt.Used)))
            {
                userRefreshToken.Invalidated = true;
            }

            await identityDbContext.SaveChangesAsync();
            return Result.Success();
        }

        /// <summary>
        ///     Generates the Token for the given user. Saves the refresh token in the database
        /// </summary>
        /// <param name="user">the user the token should be generated for</param>
        /// <returns>The AuthResponse with the token</returns>
        private async Task<AuthResponse> GenerateAuthenticationResultForUserAsync(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
            var singingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.UtcNow.Add(jwtSettings.TokenLifetime.ToTimeSpan());
            var claims = await ValidClaimsAsync(user);

            var token = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                expires: expiry,
                signingCredentials: singingCredentials
            );

            var refreshToken = new RefreshToken
            {
                JwtTokenId = token.Id,
                Token = Guid.NewGuid().ToString(),
                UserId = user.Id,
                Expires = instant.Now.Plus(Duration.FromDays(90)),
            };

            await identityDbContext.RefreshTokens.AddAsync(refreshToken);
            await identityDbContext.SaveChangesAsync();

            return AuthResponse.Success(new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.Token,
            });
        }

        private async Task<IEnumerable<Claim>> ValidClaimsAsync(AppUser user)
        {
            var identityOptions = new IdentityOptions();


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(identityOptions.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new Claim(identityOptions.ClaimsIdentity.UserNameClaimType, user.UserName),
            };

            var userClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await roleManager.FindByNameAsync(userRole);
                if (role == null)
                {
                    continue;
                }

                var roleClaims = await roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }

            return claims;
        }

        private ClaimsPrincipal PrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var modifiedTokenValidationParameters = tokenValidationParameters.Clone();
                modifiedTokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, modifiedTokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }


        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return validatedToken is JwtSecurityToken jwtSecurityToken &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }
    }
}