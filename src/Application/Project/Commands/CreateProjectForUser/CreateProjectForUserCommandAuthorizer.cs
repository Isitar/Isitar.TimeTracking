namespace Isitar.TimeTracking.Application.Project.Commands.CreateProject
{
    using System.Threading.Tasks;
    using Common.Authorization;
    using Common.Enums;
    using Common.Interfaces;
    using MediatR;

    public class CreateProjectForUserCommandAuthorizer : AbstractAuthorizer<CreateProjectForUserCommand>
    {
        private readonly IIdentityService identityService;

        public CreateProjectForUserCommandAuthorizer(ICurrentUserService currentUserService, IMediator mediator, IIdentityService identityService) : base(currentUserService, mediator)
        {
            this.identityService = identityService;
        }

        public override async Task<bool> AuthorizeAsync(CreateProjectForUserCommand request)
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