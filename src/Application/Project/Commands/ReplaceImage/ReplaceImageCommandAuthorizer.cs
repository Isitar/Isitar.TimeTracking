namespace Isitar.TimeTracking.Application.Project.Commands.ReplaceImage
{
    using System.Threading.Tasks;
    using Common.Authorization;
    using Common.Interfaces;
    using MediatR;
    using Queries.CanEditProject;

    public class ReplaceImageCommandAuthorizer : AbstractAuthorizer<ReplaceImageCommand>
    {
        public ReplaceImageCommandAuthorizer(ICurrentUserService currentUserService, IMediator mediator) : base(currentUserService, mediator) { }

        public override async Task<bool> AuthorizeAsync(ReplaceImageCommand request)
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