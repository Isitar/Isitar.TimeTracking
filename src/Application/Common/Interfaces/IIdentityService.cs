namespace Isitar.TimeTracking.Application.Common.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Entities;
    using Enums;

    public interface IIdentityService
    {
        public Task<Result> CreateUserAsync(Guid userId, string username, string password);
        public Task<Result> DeleteUserAsync(Guid userId);

        public Task<Result<string>> UsernameByGuidAsync(Guid userId);
        public Task<Result> SetPasswordAsync(Guid userId, string password);
        public Task<Result> SetUsernameAsync(Guid userId, string username);

        public Task<Result<bool>> CanAsync(Guid userId, Permission permission);
        public Task<Result> AssignPermissionAsync(Guid userId, Permission permission);
        public Task<Result> RevokePermissionAsync(Guid userId, Permission permission);
        
    }
}