namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingEntryList
{
    using System;
    using System.Collections.Generic;
    using MediatR;
    using NodaTime;
    using TimeTrackingList;

    public class TimeTrackingEntryListQuery : IRequest<TimeTrackingEntryListVm>
    {
        public ICollection<Guid> UserFilter { get; set; } = new HashSet<Guid>();
        public ICollection<Guid> ProjectFilter { get; set; } = new HashSet<Guid>();
        public Instant? From { get; set; }
        public Instant? To { get; set; }
    }
}