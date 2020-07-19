namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.ActiveTimeTrackingEntryDetail
{
    using System;
    using MediatR;
    using TimeTrackingEntryDetail;

    public class ActiveTimeTrackingEntryDetailQuery : IRequest<TimeTrackingEntryDetailVm>
    {
        public Guid UserId { get; set; }
    }
}