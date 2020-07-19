namespace Isitar.TimeTracking.Frontend.Services
{
    using System;
    using System.Threading.Tasks;
    using Application.Common.Exceptions;
    using Application.Common.Interfaces;
    using Application.TimeTrackingEntry.Queries.TimeTrackingEntryDetail;
    using Application.TimeTrackingEntry.Queries.TimeTrackingEntryRapport;
    using NodaTime;

    public class TimeTrackingService : ITimeTrackingService
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IGenericService genericService;

        public TimeTrackingService(IGenericService genericService)
        {
            this.genericService = genericService;
        }

        public Task<TimeTrackingEntryDetailVm> CurrentTimeTrackingEntryAsync(Guid userId)
        {
            try
            {
                return genericService.GetAsync<TimeTrackingEntryDetailVm>($"user/{userId}/time-tracking-entry/current");
            }
            catch (NotFoundException)
            {
                return null;
            }
        }

        public Task<TimeTrackingEntryRapportVm> RapportAsync(Guid userId, Instant @from, Instant to)
        {
            throw new NotImplementedException();
        }
    }
}