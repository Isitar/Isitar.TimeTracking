namespace Isitar.TimeTracking.Application.Project.Commands.DeleteProject
{
    using System.Threading.Tasks;
    using Common.Authorization;
    using Common.Interfaces;
    using MediatR;
    using Queries.CanEditProject;

    public class DeleteProjectCommandAuthorizer : AbstractAuthorizer<DeleteProjectCommand>
    {
        public DeleteProjectCommandAuthorizer(ICurrentUserService currentUserService, IMediator mediator) : base(currentUserService, mediator) { }

        public async override Task<bool> AuthorizeAsync(DeleteProjectCommand request)
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