namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingList
{
    using System.Collections.Generic;

    public class TimeTrackingEntryListVm
    {
        public IList<TimeTrackingEntrySlimDto> TimeTrackingEntries { get; set; }
    }
}