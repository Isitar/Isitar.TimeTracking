namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingEntryReport
{
    using System;
    using MediatR;
    using NodaTime;

    public class TimeTrackingEntryReportQuery : IRequest<TimeTrackingEntryReportVm>
    {
        public Guid UserId { get; set; }
        public Instant From { get; set; }
        public Instant To { get; set; }
    }
}