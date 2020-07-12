namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Commands.StopTrackingForUser
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using global::Common;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class StopTrackingForUserCommandHandler : IRequestHandler<StopTrackingForUserCommand>
    {
        private readonly ITimeTrackingDbContext dbContext;
        private readonly IInstant instant;

        public StopTrackingForUserCommandHandler(ITimeTrackingDbContext dbContext, IInstant instant)
        {
            this.dbContext = dbContext;
            this.instant = instant;
        }

        public async Task<Unit> Handle(StopTrackingForUserCommand request, CancellationToken cancellationToken)
        {
            var timeTrackingEntries = await dbContext.TimeTrackingEntries
                .Where(tte => tte.UserId.Equals(request.UserId) && !tte.To.HasValue)
                .ToListAsync(cancellationToken: cancellationToken);
            foreach (var timeTrackingEntry in timeTrackingEntries)
            {
                timeTrackingEntry.To = instant.Now;
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}