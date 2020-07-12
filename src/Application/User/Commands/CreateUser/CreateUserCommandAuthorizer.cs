namespace Isitar.TimeTracking.Application.User.Commands.CreateUser
{
    using System.Threading.Tasks;
    using Common.Authorization;
    using Common.Enums;
    using Common.Interfaces;
    using MediatR;

    public class CreateUserCommandAuthorizer : AbstractAuthorizer<CreateUserCommand>
    {
        private readonly IIdentityService identityService;

        public CreateUserCommandAuthorizer(ICurrentUserService currentUserService, IMediator mediator, IIdentityService identityService) : base(currentUserService, mediator)
        {
            this.identityService = identityService;
        }

        public override async Task<bool> AuthorizeAsync(CreateUserCommand request)
        {
            if (!(CurrentUserService.IsAuthenticated && CurrentUserService.UserId.HasValue))
            {
                return false;
            }

            var canResult = await identityService.CanAsync(CurrentUserService.UserId.Value, Permissions.Admin);
            return canResult.Successful && canResult.Data;
        }
    }
}