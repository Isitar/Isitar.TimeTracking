namespace Isitar.TimeTracking.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using NodaTime;

    public interface IAuditableEntity : IEntity
    {
        public User CreatedBy { get; set; }
        public Guid? CreatedById { get; set; }
        public Instant? CreatedAt { get; set; }

        public User UpdatedBy { get; set; }
        public Guid? UpdatedById { get; set; }
        public Instant? UpdatedAt { get; set; }

        public ICollection<AuditTrailEntry> AuditTrailEntries { get; }
        
    }
}