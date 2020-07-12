namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Commands.StartTrackingForProject
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using Domain.Entities;
    using global::Common;
    using MediatR;
    using StopTrackingForUser;

    public class StartTrackingForProjectCommandHandler : IRequestHandler<StartTrackingForProjectCommand>
    {
        private readonly IMediator mediator;
        private readonly ITimeTrackingDbContext dbContext;
        private readonly IInstant instant;
        private readonly ICurrentUserService currentUserService;
        private readonly IIdentityService identityService;

        public StartTrackingForProjectCommandHandler(
            IMediator mediator,
            ITimeTrackingDbContext dbContext,
            IInstant instant,
            ICurrentUserService currentUserService,
            IIdentityService identityService)
        {
            this.mediator = mediator;
            this.dbContext = dbContext;
            this.instant = instant;
            this.currentUserService = currentUserService;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(StartTrackingForProjectCommand request, CancellationToken cancellationToken)
        {
            // stop old tracking
            await mediator.Send(new StopTrackingForUserCommand
            {
                UserId = request.UserId
            }, cancellationToken);

            // create new tracking entry
            var timeTrackingEntry = new TimeTrackingEntry
            {
                From = instant.Now,
                Id = request.Id,
                ProjectId = request.ProjectId,
                UserId = request.UserId,
                To = null
            };

            await dbContext.TimeTrackingEntries.AddAsync(timeTrackingEntry, cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}