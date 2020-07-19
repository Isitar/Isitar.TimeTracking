namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingEntryRapport
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class TimeTrackingEntryRapportQueryHandler : IRequestHandler<TimeTrackingEntryRapportQuery, TimeTrackingEntryRapportVm>
    {
        private readonly ITimeTrackingDbContext dbContext;

        public TimeTrackingEntryRapportQueryHandler(ITimeTrackingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public async Task<TimeTrackingEntryRapportVm> Handle(TimeTrackingEntryRapportQuery request, CancellationToken cancellationToken)
        {
            var timeTrackingEntryRapports = await dbContext
                .TimeTrackingEntries
                .Where(tte => tte.From >= request.From && tte.To.HasValue && tte.To.Value <= request.To)
                .Where(tte => tte.UserId.Equals(request.UserId))
                .GroupBy(tte => tte.ProjectId)
                .Select(g => new TimeTrackingEntryRapportDto
                {
                    ProjectId = g.Key,
                    TotalSeconds = g.Sum(e => e.To.Value.Minus(e.From).TotalSeconds),
                })
                .ToListAsync(cancellationToken);
            
            return new TimeTrackingEntryRapportVm
            {
                From = request.From,
                To = request.To,
                UserId = request.UserId,
                TimeTrackingEntryRapports = timeTrackingEntryRapports
            };
        }
    }
}