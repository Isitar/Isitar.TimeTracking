namespace Isitar.TimeTracking.Frontend.Services
{
    using System;
    using System.Threading.Tasks;
    using Application.TimeTrackingEntry.Queries.TimeTrackingEntryDetail;
    using Application.TimeTrackingEntry.Queries.TimeTrackingEntryReport;
    using Exceptions;
    using NodaTime;

    public class TimeTrackingService : ITimeTrackingService
    {
        private readonly IGenericService genericService;

        public TimeTrackingService(IGenericService genericService)
        {
            this.genericService = genericService;
        }

        public async Task<TimeTrackingEntryDetailVm> CurrentTimeTrackingEntryAsync(Guid userId)
        {
            try
            {
                return await genericService.GetAsync<TimeTrackingEntryDetailVm>($"user/{userId}/time-tracking-entry/current");
            }
            catch (HttpNotFoundException)
            {
                return null;
            }
        }

        public async Task<TimeTrackingEntryReportVm> ReportAsync(Guid userId, Instant @from, Instant to)
        {
            try
            {
                var url = $"user/{userId}/time-tracking-report?from={from.ToString()}&to={to.ToString()}";
                return await genericService.GetAsync<TimeTrackingEntryReportVm>(url);
            }
            catch (HttpNotFoundException)
            {
                return null;
            }
        }
    }
}