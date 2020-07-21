namespace Isitar.TimeTracking.Api.Requests.V1.UserTimeTrackingEntry
{
    using System;
    using NodaTime;

    public class AllTimeTrackingEntriesFilter
    {
        public Instant? From { get; set; }
        public Instant? To { get; set; }
        public Guid[] ProjectIds { get; set; } = new Guid[0];
    }
}