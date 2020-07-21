namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingEntryReport
{
    using System;

    public class TimeTrackingEntryReportDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public double TotalSeconds { get; set; }
    }
}