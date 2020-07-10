namespace Isitar.TimeTracking.Application.Project.Commands.DeleteProject
{
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Authorization;
    using Common.Interfaces;

    public class DeleteProjectCommandAuthorizer : AbstractAuthorizer<DeleteProjectCommand>
    {
        private readonly ITimeTrackingDbContext dbContext;

        public DeleteProjectCommandAuthorizer(ICurrentUserService currentUserService, IIdentityService identityService, ITimeTrackingDbContext dbContext) : base(currentUserService, identityService)
        {
            this.dbContext = dbContext;
        }

        public override async Task<bool> AuthorizeAsync(DeleteProjectCommand request)
        {
            if (await IsCurrentUserAdminAsync())
            {
                return true;
            }

            if (!CurrentUserService.IsAuthenticated)
            {
                return false;
            }

            return dbContext.Projects.Any(p => p.Id.Equals(request.Id) && p.UserId.Equals(CurrentUserService.UserId.Value));
        }
    }
}