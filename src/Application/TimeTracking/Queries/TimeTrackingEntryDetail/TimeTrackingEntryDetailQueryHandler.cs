namespace Isitar.TimeTracking.Application.TimeTracking.Queries.TimeTrackingEntryDetail
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

    public class TimeTrackingEntryDetailQueryHandler : IRequestHandler<TimeTrackingEntryDetailQuery, TimeTrackingEntryDetailVm>
    {
        private readonly ITimeTrackingDbContext dbContext;
        private readonly IMapper mapper;

        public TimeTrackingEntryDetailQueryHandler(ITimeTrackingDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<TimeTrackingEntryDetailVm> Handle(TimeTrackingEntryDetailQuery request, CancellationToken cancellationToken)
        {
            var vm = await dbContext.TimeTrackingEntries
                .Where(tte => tte.Id.Equals(request.Id))
                .ProjectTo<TimeTrackingEntryDetailVm>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken: cancellationToken);
            if (null == vm)
            {
                throw new NotFoundException(Translation.TimeTrackingEntry, request.Id);
            }

            return vm;
        }
    }
}