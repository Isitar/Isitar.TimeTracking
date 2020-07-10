namespace Isitar.TimeTracking.Application.TimeTracking.Commands.StopTrackingForUser
{
    using System.Threading.Tasks;
    using Common.Authorization;
    using Common.Interfaces;

    public class StopTrackingForUserCommandAuthorizer : AbstractAuthorizer<StopTrackingForUserCommand>
    {
        public StopTrackingForUserCommandAuthorizer(ICurrentUserService currentUserService, IIdentityService identityService) : base(currentUserService, identityService) { }

        public override async Task<bool> AuthorizeAsync(StopTrackingForUserCommand request)
        {
            if (await IsCurrentUserAdminAsync())
            {
                return true;
            }

            if (!CurrentUserService.IsAuthenticated)
            {
                return false;
            }

            return CurrentUserService.UserId.Equals(request.UserId);
        }
    }
}