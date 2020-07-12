namespace Isitar.TimeTracking.Application.Project.Commands.UpdateProjectName
{
    using System.Threading.Tasks;
    using Common.Authorization;
    using Common.Interfaces;
    using MediatR;
    using Queries.CanEditProject;

    public class UpdateProjectNameCommandAuthorizer : AbstractAuthorizer<UpdateProjectNameCommand>
    {
        public UpdateProjectNameCommandAuthorizer(ICurrentUserService currentUserService, IMediator mediator) : base(currentUserService, mediator) { }

        public override async Task<bool> AuthorizeAsync(UpdateProjectNameCommand request)
        {
            if (!(CurrentUserService.IsAuthenticated && CurrentUserService.UserId.HasValue))
            {
                return false;
            }

            return await Mediator.Send(new CanEditProjectQuery
            {
                UserId = CurrentUserService.UserId.Value,
                ProjectId = request.Id,
            });
        }
    }
}