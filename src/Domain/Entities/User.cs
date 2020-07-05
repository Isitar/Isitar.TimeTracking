namespace Isitar.TimeTracking.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using NodaTime;

    public class User : IAuditableEntity
    {
        public string Acronym { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string Locale { get; set; }
        
        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();

        #region IAuditableEntity

        public Guid Id { get; set; }
        public User CreatedBy { get; set; }
        public Guid? CreatedById { get; set; }
        public Instant? CreatedAt { get; set; }
        public User UpdatedBy { get; set; }
        public Guid? UpdatedById { get; set; }
        public Instant? UpdatedAt { get; set; }
        public ICollection<AuditTrailEntry> AuditTrailEntries { get; } = new HashSet<AuditTrailEntry>();

        #endregion
    }
}