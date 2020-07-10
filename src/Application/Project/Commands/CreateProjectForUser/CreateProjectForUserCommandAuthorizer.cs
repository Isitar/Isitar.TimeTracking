namespace Isitar.TimeTracking.Application.Project.Commands.CreateProject
{
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Authorization;
    using Common.Interfaces;

    public class CreateProjectForUserCommandAuthorizer : AbstractAuthorizer<CreateProjectForUserCommand>
    {
        private readonly ITimeTrackingDbContext dbContext;

        public CreateProjectForUserCommandAuthorizer(ICurrentUserService currentUserService, IIdentityService identityService, ITimeTrackingDbContext dbContext) : base(currentUserService, identityService)
        {
            this.dbContext = dbContext;
        }

        public override async Task<bool> AuthorizeAsync(CreateProjectForUserCommand request)
        {
            if (await IsCurrentUserAdminAsync())
            {
                return true;
            }

            if (!CurrentUserService.IsAuthenticated)
            {
                return false;
            }

            return dbContext.Projects.Any(p => p.Id.Equals(request.Id) && p.UserId.Equals(CurrentUserService.UserId));
        }
    }
}