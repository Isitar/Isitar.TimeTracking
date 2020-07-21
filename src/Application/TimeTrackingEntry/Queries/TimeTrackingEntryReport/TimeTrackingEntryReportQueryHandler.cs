namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingEntryReport
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class TimeTrackingEntryReportQueryHandler : IRequestHandler<TimeTrackingEntryReportQuery, TimeTrackingEntryReportVm>
    {
        private readonly ITimeTrackingDbContext dbContext;
        private readonly ILogger<TimeTrackingEntryReportQueryHandler> logger;

        public TimeTrackingEntryReportQueryHandler(ITimeTrackingDbContext dbContext, ILogger<TimeTrackingEntryReportQueryHandler> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<TimeTrackingEntryReportVm> Handle(TimeTrackingEntryReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var timeTrackingEntryRapports = dbContext
                    .TimeTrackingEntries
                    .Include(tte => tte.Project)
                    .Where(tte => tte.From >= request.From && tte.To.HasValue && tte.To.Value <= request.To)
                    .Where(tte => tte.UserId.Equals(request.UserId))
                    .Select(tte => new {ProjectId = tte.ProjectId, ProjectName = tte.Project.Name, From = tte.From, To = tte.To.Value})
                    .AsEnumerable()
                    .GroupBy(tte => new {tte.ProjectId, tte.ProjectName})
                    .Select(g => new TimeTrackingEntryReportDto
                    {
                        ProjectId = g.Key.ProjectId,
                        ProjectName = g.Key.ProjectName,
                        TotalSeconds = g.Sum(e => e.To.Minus(e.From).TotalSeconds),
                    })
                    .ToList();

                return new TimeTrackingEntryReportVm
                {
                    From = request.From,
                    To = request.To,
                    UserId = request.UserId,
                    TimeTrackingEntryReports = timeTrackingEntryRapports
                };
            }
            catch (Exception e)
            {
                logger.LogError(e, "error generating report");
                throw;
            }
        }
    }
}