namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingEntryRapport
{
    using System;
    using MediatR;
    using NodaTime;

    public class TimeTrackingEntryRapportQuery : IRequest<TimeTrackingEntryRapportVm>
    {
        public Guid UserId { get; set; }
        public Instant From { get; set; }
        public Instant To { get; set; }
    }
}