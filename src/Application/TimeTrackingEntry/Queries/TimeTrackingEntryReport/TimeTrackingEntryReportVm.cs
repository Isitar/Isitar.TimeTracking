namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingEntryReport
{
    using System;
    using System.Collections.Generic;
    using NodaTime;

    public class TimeTrackingEntryReportVm
    {
        public Guid UserId { get; set; }
        public Instant From { get; set; }
        public Instant To { get; set; }
        public IEnumerable<TimeTrackingEntryReportDto> TimeTrackingEntryReports { get; set; }
    }
}