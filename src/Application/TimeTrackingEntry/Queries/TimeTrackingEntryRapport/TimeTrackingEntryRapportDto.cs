namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingEntryRapport
{
    using System;
    using NodaTime;

    public class TimeTrackingEntryRapportDto
    {
        public Guid ProjectId { get; set; }
        public Double TotalSeconds { get; set; }
    }
}