namespace Isitar.TimeTracking.Application.Project.Queries.CanEditProject
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Enums;
    using Common.Interfaces;
    using MediatR;

    public class CanEditProjectQueryHandler : IRequestHandler<CanEditProjectQuery, bool>
    {
        private readonly ITimeTrackingDbContext dbContext;
        private readonly IIdentityService identityService;

        public CanEditProjectQueryHandler(ITimeTrackingDbContext dbContext, IIdentityService identityService)
        {
            this.dbContext = dbContext;
            this.identityService = identityService;
        }

        public async Task<bool> Handle(CanEditProjectQuery request, CancellationToken cancellationToken)
        {
            var canAdmin = await identityService.CanAsync(request.UserId, Permissions.Admin);
            if (canAdmin.Successful && canAdmin.Data)
            {
                return true;
            }

            return dbContext.Projects.Any(p => p.UserId.Equals(request.UserId) && p.Id.Equals(request.ProjectId));
        }
    }
}