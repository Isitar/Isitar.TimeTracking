namespace Isitar.TimeTracking.Infrastructure.Identity
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Application.Common.Entities;
    using Application.Common.Enums;
    using Application.Common.Exceptions;
    using Application.Common.Interfaces;
    using Common.Resources;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ICurrentUserService currentUserService;

        public IdentityService(UserManager<AppUser> userManager, ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
        }

        private async Task<bool> IsCurrentUserAdminAsync()
        {
            var currentUserId = currentUserService.UserId;
            if (!currentUserId.HasValue)
            {
                return false;
            }

            var result = await CanAsync(currentUserId.Value, Permission.Admin);
            return result.Successful && result.Data;
        }

        private bool IsCurrentUser(Guid id)
        {
            var currentUserId = currentUserService.UserId;
            return currentUserId.HasValue && currentUserId.Value.Equals(id);
        }

        public async Task<Result> CreateUserAsync(Guid userId, string username, string password)
        {
            // check auth
            if (!(await IsCurrentUserAdminAsync()))
            {
                throw new UnauthorizedException();
            }

            var user = new AppUser
            {
                UserName = username,
                Email = username,
                Id = userId,
            };
            var result = await userManager.CreateAsync(user, password);
            return result.ToResult();
        }

        public async Task<Result> DeleteUserAsync(Guid userId)
        {
            // check auth
            if (!(await IsCurrentUserAdminAsync()))
            {
                throw new UnauthorizedException();
            }
            
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(AppUser user)
        {
            // check auth
            if (!(await IsCurrentUserAdminAsync()))
            {
                throw new UnauthorizedException();
            }
            
            var result = await userManager.DeleteAsync(user);

            return result.ToResult();
        }

        public async Task<Result<string>> UsernameByGuidAsync(Guid userId)
        {
            var userResult = await FindUserAsync(userId);
            if (!userResult.Successful)
            {
                return Result<string>.Failure(userResult.Errors);
            }

            var user = userResult.Data;

            return Result<string>.Success(user.UserName);
        }

        public async Task<Result> SetPasswordAsync(Guid userId, string password)
        {
            // check auth
            if (!await IsCurrentUserAdminAsync() || !IsCurrentUser(userId))
            {
                throw new UnauthorizedException();
            }
            
            var userResult = await FindUserAsync(userId);
            if (!userResult.Successful)
            {
                return Result.Failure(userResult.Errors);
            }

            var user = userResult.Data;


            await userManager.RemovePasswordAsync(user);
            await userManager.AddPasswordAsync(user, password);
            return Result.Success();
        }

        public async Task<Result> SetUsernameAsync(Guid userId, string username)
        {
            // check auth
            if (!await IsCurrentUserAdminAsync() ||  !IsCurrentUser(userId))
            {
                throw new UnauthorizedException();
            }
            var userResult = await FindUserAsync(userId);
            if (!userResult.Successful)
            {
                return Result.Failure(userResult.Errors);
            }

            var user = userResult.Data;

            user.UserName = username;
            user.Email = username;
            await userManager.UpdateAsync(user);
            return Result.Success();
        }

        public async Task<Result<bool>> CanAsync(Guid userId, Permission permission)
        {
            var userResult = await FindUserAsync(userId);
            if (!userResult.Successful)
            {
                return Result<bool>.Failure(userResult.Errors);
            }

            var user = userResult.Data;
            var claims = await userManager.GetClaimsAsync(user);

            return Result<bool>.Success(claims.Any(c => c.Type.Equals(CustomClaimTypes.PermissionClaimType) && c.Value.Equals(permission.ToString())));
        }

        public async Task<Result> AssignPermissionAsync(Guid userId, Permission permission)
        {
            // check auth
            if (!(await IsCurrentUserAdminAsync()))
            {
                throw new UnauthorizedException();
            }
            
            var userResult = await FindUserAsync(userId);
            if (!userResult.Successful)
            {
                return Result<bool>.Failure(userResult.Errors);
            }

            var user = userResult.Data;
            var res = await userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.PermissionClaimType, permission.ToString()));
            return res.ToResult();
        }

        public async Task<Result> RevokePermissionAsync(Guid userId, Permission permission)
        {
            // check auth
            if (!(await IsCurrentUserAdminAsync()))
            {
                throw new UnauthorizedException();
            }
            
            var userResult = await FindUserAsync(userId);
            if (!userResult.Successful)
            {
                return Result<bool>.Failure(userResult.Errors);
            }

            var user = userResult.Data;

            var claims = await userManager.GetClaimsAsync(user);
            var relevantClaims = claims.Where(c => c.Type.Equals(CustomClaimTypes.PermissionClaimType) && c.Value.Equals(permission.ToString()));
            var res = await userManager.RemoveClaimsAsync(user, relevantClaims);

            return res.ToResult();
        }

        private async Task<Result<AppUser>> FindUserAsync(Guid userId)
        {
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if (null == user)
            {
                return Result<AppUser>.Failure(new[]
                {
                    Translation.NotFoundException
                        .Replace("{name}", Translation.User)
                        .Replace("{key}", userId.ToString())
                });
            }

            return Result<AppUser>.Success(user);
        }
    }
}