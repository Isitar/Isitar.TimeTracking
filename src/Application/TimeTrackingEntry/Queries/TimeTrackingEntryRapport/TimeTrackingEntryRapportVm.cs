namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingEntryRapport
{
    using System;
    using System.Collections.Generic;
    using NodaTime;

    public class TimeTrackingEntryRapportVm
    {
        public Guid UserId { get; set; }
        public Instant From { get; set; }
        public Instant To { get; set; }
        public IEnumerable<TimeTrackingEntryRapportDto> TimeTrackingEntryRapports { get; set; }
    }
}