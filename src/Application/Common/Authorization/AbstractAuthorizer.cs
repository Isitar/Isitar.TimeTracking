namespace Isitar.TimeTracking.Application.Common.Authorization
{
    using System.Threading.Tasks;
    using Enums;
    using Interfaces;

    public abstract class AbstractAuthorizer<T> : IAuthorizer<T>
    {
        protected readonly ICurrentUserService CurrentUserService;
        protected  readonly IIdentityService IdentityService;

        public AbstractAuthorizer(ICurrentUserService currentUserService, IIdentityService identityService)
        {
            this.CurrentUserService = currentUserService;
            this.IdentityService = identityService;
        }

        protected async Task<bool> IsCurrentUserAdminAsync()
        {
            var currentUserId = CurrentUserService.UserId;
            if (!currentUserId.HasValue)
            {
                return false;
            }

            var result = await IdentityService.CanAsync(currentUserId.Value, Permissions.Admin);
            return result.Successful && result.Data;
        }
        
        public abstract Task<bool> AuthorizeAsync(T request);
    }
}