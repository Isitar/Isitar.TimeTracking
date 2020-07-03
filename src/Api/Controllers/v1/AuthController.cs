﻿namespace Isitar.TimeTracking.Api.Controllers.v1
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Attributes;
    using Infrastructure.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Requests.v1;
    using Requests.v1.auth;
    using Routes.v1;

    public class AuthController : ApiController
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IIdentityService identityService;

        public AuthController(IIdentityService identityService,  IAuthorizationService authorizationService)
        {
            this.identityService = identityService;
            this.authorizationService = authorizationService;
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
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            var resp = await identityService.RefreshAsync(refreshTokenRequest.RefreshToken, refreshTokenRequest.JwtToken);
            if (!resp.Success)
            {
                return BadRequest(resp.ErrorMessages);
            }

            return Ok(new AuthSuccessResponse
            {
                Token = resp.Data.Token,
                RefreshToken = resp.Data.RefreshToken
            });
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Auth.Login, Name = nameof(AuthController) + "/" + nameof(TokenAsync))]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> TokenAsync([FromBody] LoginRequest loginRequest)
        {
            var resp = await identityService.LoginAsync(loginRequest.Username, loginRequest.Password);
            if (!resp.Success)
            {
                return BadRequest(resp.ErrorMessages
                );
            }

            return Ok(new AuthSuccessResponse
            {
                Token = resp.Data.Token,
                RefreshToken = resp.Data.RefreshToken
            });
        }

        [HttpPost(ApiRoutes.Auth.Logout, Name = nameof(AuthController) + "/" + nameof(LogoutAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> LogoutAsync(Guid? userId)
        {
            var loggedInUserId = CoreUserId();
            var isAdmin = (await authorizationService.AuthorizeAsync(User, ClaimPermission.Admin)).Succeeded;
            if (!isAdmin && userId.HasValue && !userId.Value.Equals(loggedInUserId))
            {
                return Unauthorized(new[] {Resources.NotAllowed});
            }

            if (!userId.HasValue)
            {
                userId = loggedInUserId;
            }

            if (null == userId)
            {
                return BadRequest(Resources.UserDoesNotExist);
            }

            var response = await identityService.LogoutAsync(userId.Value);
            return response.Success ? (IActionResult) Ok() : BadRequest();
        }

        [Authorize(Policy = ClaimPermission.Admin)]
        [HttpPost(ApiRoutes.Auth.ToggleRole, Name = nameof(AuthController) + "/" + nameof(ToggleRoleAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> ToggleRoleAsync(ToggleRoleRequest toggleRoleRequest)
        {
            if (toggleRoleRequest.UserId.Equals(CoreUserId()))
            {
                return BadRequest(new[] {Resources.CannotSetOwnRoles});
            }

            var response = await identityService.ToggleRoleAsync(toggleRoleRequest.UserId, toggleRoleRequest.RoleName);
            if (!response.Success)
            {
                return BadRequest(response.ErrorMessages);
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
            var result = await identityService.UserRolesAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessages);
            }

            return Ok(result.Data);
        }

        [AllowAnonymous]
        [HttpPost(ApiRoutes.Auth.ResetPassword, Name = nameof(AuthController) + "/" + nameof(ResetUserPasswordAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesBadRequestResponse]
        public async Task<IActionResult> ResetUserPasswordAsync(ResetPasswordRequest passwordResetRequest)
        {
            var userIdResponse = await identityService.FindPersonIdByEmailAsync(passwordResetRequest.Email);
            if (!userIdResponse.Success)
            {
                return BadRequest();
            }

            var response = await identityService.ResetPasswordAndSendMailAsync(userIdResponse.Data, mailService, "https://my.bpw.ch", passwordResetRequest.MailTemplate);
            if (!response.Success)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}