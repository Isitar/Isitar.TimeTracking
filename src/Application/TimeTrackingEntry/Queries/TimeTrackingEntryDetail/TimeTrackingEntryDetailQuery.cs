namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingEntryDetail
{
    using System;
    using MediatR;

    public class TimeTrackingEntryDetailQuery : IRequest<TimeTrackingEntryDetailVm>
    {
        public Guid Id { get; set; }
    }
}