namespace Isitar.TimeTracking.Frontend.Services
{
    using System;
    using System.Threading.Tasks;
    using Application.TimeTrackingEntry.Queries.TimeTrackingEntryDetail;
    using Application.TimeTrackingEntry.Queries.TimeTrackingEntryReport;
    using NodaTime;

    public interface ITimeTrackingService
    {
        public Task<TimeTrackingEntryDetailVm> CurrentTimeTrackingEntryAsync(Guid userId);
        public Task<TimeTrackingEntryReportVm> ReportAsync(Guid userId, Instant from, Instant to);
    }
}