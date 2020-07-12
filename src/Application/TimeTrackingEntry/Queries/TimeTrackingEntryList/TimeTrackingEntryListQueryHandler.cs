namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingEntryList
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using TimeTrackingList;

    public class TimeTrackingEntryListQueryHandler : IRequestHandler<TimeTrackingEntryListQuery, TimeTrackingEntryListVm>
    {
        private readonly ITimeTrackingDbContext dbContext;
        private readonly IMapper mapper;

        public TimeTrackingEntryListQueryHandler(ITimeTrackingDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<TimeTrackingEntryListVm> Handle(TimeTrackingEntryListQuery request, CancellationToken cancellationToken)
        {
            var q = dbContext.TimeTrackingEntries.AsQueryable();
            if (request.UserFilter.Any())
            {
                q = q.Where(tte => request.UserFilter.Contains(tte.UserId));
            }

            if (request.ProjectFilter.Any())
            {
                q = q.Where(tte => request.ProjectFilter.Contains(tte.ProjectId));
            }

            if (request.From.HasValue)
            {
                q = q.Where(tte => tte.From >= request.From.Value);
            }

            if (request.To.HasValue)
            {
                q = q.Where(tte => tte.To <= request.To.Value);
            }

            var timeTrackingEntries = await q
                .ProjectTo<TimeTrackingEntrySlimDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new TimeTrackingEntryListVm
            {
                TimeTrackingEntries = timeTrackingEntries,
            };
        }
    }
}