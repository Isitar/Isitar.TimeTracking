namespace Isitar.TimeTracking.Api.Requests.V1.UserTimeTrackingEntry
{
    using NodaTime;

    public class TimeTrackingReportRequest
    {
        public Instant From { get; set; }
        public Instant To { get; set; }
    }
}