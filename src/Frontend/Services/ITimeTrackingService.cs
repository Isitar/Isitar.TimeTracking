namespace Isitar.TimeTracking.Frontend.Services
{
    using System;
    using System.Threading.Tasks;
    using Application.TimeTrackingEntry.Queries.TimeTrackingEntryDetail;
    using Application.TimeTrackingEntry.Queries.TimeTrackingEntryRapport;
    using NodaTime;

    public interface ITimeTrackingService
    {
        public Task<TimeTrackingEntryDetailVm> CurrentTimeTrackingEntryAsync(Guid userId);
        public Task<TimeTrackingEntryRapportVm> RapportAsync(Guid userId, Instant from, Instant to);
    }
}