namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.ActiveTimeTrackingEntryDetail
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Exceptions;
    using Common.Interfaces;
    using global::Common.Resources;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using TimeTrackingEntryDetail;

    public class ActiveTimeTrackingEntryDetailQueryHandler : IRequestHandler<ActiveTimeTrackingEntryDetailQuery, TimeTrackingEntryDetailVm>
    {
        private readonly ITimeTrackingDbContext dbContext;
        private readonly IMapper mapper;

        public ActiveTimeTrackingEntryDetailQueryHandler(ITimeTrackingDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<TimeTrackingEntryDetailVm> Handle(ActiveTimeTrackingEntryDetailQuery request, CancellationToken cancellationToken)
        {
            var vm = await dbContext.TimeTrackingEntries
                .Where(tte => !tte.To.HasValue)
                .Where(tte => tte.UserId.Equals(request.UserId))
                .ProjectTo<TimeTrackingEntryDetailVm>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
            
            if (null == vm)
            {
                throw new NotFoundException(Translation.TimeTrackingEntry, null);
            }

            return vm;
        }
        
        
    }
}