namespace Isitar.TimeTracking.Api.Controllers.V1
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Attributes;
    using Common.Resources;
    using Infrastructure.Identity.Services.TokenService;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Requests.V1.Auth;
    using Routes.V1;

    public class AuthController : ApiController
    {
        private readonly IIdentityService identityService;
        private readonly ITokenService tokenService;
        private readonly ICurrentUserService currentUserService;

        public AuthController(IIdentityService identityService, ITokenService tokenService, ICurrentUserService currentUserService)
        {
            this.identityService = identityService;
            this.tokenService = tokenService;
            this.currentUserService = currentUserService;
        }

        [HttpPost(ApiRoutes.Auth.ChangePassword, Name = nameof(AuthController) + "/" + nameof(ChangePasswordAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> ChangePasswordAsync(Guid id, [FromBody] ChangePasswordRequest changePasswordRequest)
        {
            var res = await identityService.SetPasswordAsync(id, changePasswordRequest.NewPassword);
            if (!res.Successful)
            {
                return BadRequest(res.Errors);
            }

            return Ok();
        }

        [HttpPost(ApiRoutes.Auth.ChangeUsername, Name = nameof(AuthController) + "/" + nameof(ChangeUsernameAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> ChangeUsernameAsync(Guid id, [FromBody] ChangeUsernameRequest changeUsernameRequest)
        {
            var res = await identityService.SetUsernameAsync(id, changeUsernameRequest.NewUsername);
            if (!res.Successful)
            {
                return BadRequest(res.Errors);
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Auth.Refresh, Name = nameof(AuthController) + "/" + nameof(RefreshTokenAsync))]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var resp = await tokenService.RefreshAsync(refreshTokenRequest.RefreshToken, refreshTokenRequest.JwtToken);
            if (!resp.Successful)
            {
                return BadRequest(resp.Errors);
            }

            return Ok(resp.Data);
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Auth.Login, Name = nameof(AuthController) + "/" + nameof(TokenAsync))]
        [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> TokenAsync([FromBody] LoginRequest loginRequest)
        {
            var resp = await tokenService.LoginAsync(loginRequest.Username, loginRequest.Password);
            if (!resp.Successful)
            {
                return BadRequest(resp.Errors);
            }

            return Ok(resp.Data);
        }

        [HttpPost(ApiRoutes.Auth.Logout, Name = nameof(AuthController) + "/" + nameof(LogoutAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> LogoutAsync(Guid? userId)
        {
            if (!userId.HasValue)
            {
                userId = currentUserService.UserId;
            }

            if (null == userId)
            {
                return BadRequest(Translation.NotFoundException.Replace("{name}", Translation.User));
            }

            var response = await tokenService.LogoutAsync(userId.Value);
            return response.Successful ? (IActionResult) Ok() : BadRequest();
        }

        [HttpPost(ApiRoutes.Auth.AddRole, Name = nameof(AuthController) + "/" + nameof(AddRoleAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> AddRoleAsync(Guid id, RoleRequest roleRequest)
        {
            var response = await identityService.AssignRoleAsync(id, roleRequest.RoleName);
            if (!response.Successful)
            {
                return BadRequest(response.Errors);
            }

            return Ok();
        }

        [HttpDelete(ApiRoutes.Auth.RemoveRole, Name = nameof(AuthController) + "/" + nameof(RemoveRoleAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> RemoveRoleAsync(Guid id, string roleName)
        {
            var response = await identityService.RevokeRoleAsync(id, roleName);
            if (!response.Successful)
            {
                return BadRequest(response.Errors);
            }

            return Ok();
        }

        /// <summary>
        ///     Returns all role names for a given person
        /// </summary>
        /// <param name="id">the person id</param>
        /// <returns></returns>
        [HttpGet(ApiRoutes.Auth.UserRoles, Name = nameof(AuthController) + "/" + nameof(UserRolesAsync))]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<ActionResult<IEnumerable<string>>> UserRolesAsync(Guid id)
        {
            var result = await identityService.RolesAsync(id);
            if (!result.Successful)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result.Data);
        }
    }
}