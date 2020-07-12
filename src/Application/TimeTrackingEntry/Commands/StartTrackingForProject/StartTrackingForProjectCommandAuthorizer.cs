namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Commands.StartTrackingForProject
{
    using System.Threading.Tasks;
    using Common.Authorization;
    using Common.Interfaces;
    using MediatR;
    using Project.Queries.CanEditProject;

    public class StartTrackingForProjectCommandAuthorizer : AbstractAuthorizer<StartTrackingForProjectCommand>
    {
        public StartTrackingForProjectCommandAuthorizer(ICurrentUserService currentUserService, IMediator mediator) : base(currentUserService, mediator) { }

        public override async Task<bool> AuthorizeAsync(StartTrackingForProjectCommand request)
        {
            if (!(CurrentUserService.IsAuthenticated && CurrentUserService.UserId.HasValue))
            {
                return false;
            }

            return await Mediator.Send(new CanEditProjectQuery
            {
                UserId = CurrentUserService.UserId.Value,
                ProjectId = request.ProjectId,
            });
        }
    }
}