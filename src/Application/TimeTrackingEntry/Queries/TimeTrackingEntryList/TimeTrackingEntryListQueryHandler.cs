namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingEntryList
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using TimeTrackingList;

    public class TimeTrackingEntryListQueryHandler : IRequestHandler<TimeTrackingEntryListQuery, TimeTrackingEntryListVm>
    {
        public Task<TimeTrackingEntryListVm> Handle(TimeTrackingEntryListQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}