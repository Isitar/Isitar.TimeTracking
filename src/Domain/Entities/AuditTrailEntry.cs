namespace Isitar.TimeTracking.Domain.Entities
{
    using System;
    using NodaTime;

    public class AuditTrailEntry
    {
        public Guid Id { get; set; }
        public Instant When { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}