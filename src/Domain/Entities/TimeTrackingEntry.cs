namespace Isitar.TimeTracking.Domain.Entities
{
    using System;
    using NodaTime;

    public class TimeTrackingEntry : IEntity
    {
        public virtual User User { get; set; }
        public Guid UserId { get; set; }

        public Instant From { get; set; }
        public Instant? To { get; set; }

        public virtual Project Project { get; set; }
        public Guid ProjectId { get; set; }

        #region IEntity

        public Guid Id { get; set; }

        #endregion
    }
}