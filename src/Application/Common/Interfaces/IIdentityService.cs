namespace Isitar.TimeTracking.Application.Common.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Entities;
    using Enums;

    public interface IIdentityService
    {
        public Task<Result> CreateUserAsync(Guid userId, string username, string email, string password);
        public Task<Result> DeleteUserAsync(Guid userId);

        public Task<Result<string>> UsernameByGuidAsync(Guid userId);
        public Task<Result> SetPasswordAsync(Guid userId, string password);
        public Task<Result> SetUsernameAsync(Guid userId, string username);
        public Task<Result> SetEmailAsync(Guid userId, string email);

        public Task<Result<bool>> CanAsync(Guid userId, string permission);
        public Task<Result> AssignRoleAsync(Guid userId, string roleName);
        public Task<Result> RevokeRoleAsync(Guid userId, string roleName);
        public Task<Result<IEnumerable<string>>> RolesAsync(Guid userId);
    }
}