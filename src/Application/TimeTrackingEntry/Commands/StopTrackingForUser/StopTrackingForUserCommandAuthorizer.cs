namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Commands.StopTrackingForUser
{
    using System.Threading.Tasks;
    using Common.Authorization;
    using Common.Enums;
    using Common.Interfaces;
    using MediatR;

    public class StopTrackingForUserCommandAuthorizer : AbstractAuthorizer<StopTrackingForUserCommand>
    {
        private readonly IIdentityService identityService;

        public StopTrackingForUserCommandAuthorizer(ICurrentUserService currentUserService, IMediator mediator, IIdentityService identityService) : base(currentUserService, mediator)
        {
            this.identityService = identityService;
        }

        public override async Task<bool> AuthorizeAsync(StopTrackingForUserCommand request)
        {
            if (!(CurrentUserService.IsAuthenticated && CurrentUserService.UserId.HasValue))
            {
                return false;
            }

            if (CurrentUserService.UserId.Equals(request.UserId))
            {
                return true;
            }

            var canResult = await identityService.CanAsync(CurrentUserService.UserId.Value, Permissions.Admin);
            return canResult.Successful && canResult.Data;
        }
    }
}