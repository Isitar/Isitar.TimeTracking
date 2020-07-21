namespace Isitar.TimeTracking.Frontend.Components.TimeTrackingEntry
{
    using System;
    using System.Threading.Tasks;
    using Application.TimeTrackingEntry.Queries.TimeTrackingEntryReport;
    using global::Common;
    using Microsoft.AspNetCore.Components;
    using NodaTime;
    using Services;

    public class TimeTrackingEntryReportBase : ComponentBase
    {
        private Guid userId;
        private Instant? to;
        private Instant? from;

        [Parameter]
        public Instant? From
        {
            get
            {
                if (@from.HasValue)
                {
                    return @from;
                }

                var tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();
                var startOfDay = tz.AtStartOfDay(InstantProvider.Now.InZone(tz).Date);
                return startOfDay.ToInstant();
            }
            set
            {
                @from = value;
                FilterChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Parameter]
        public Instant? To
        {
            get
            {
                if (to.HasValue)
                {
                    return to;
                }

                if (@from.HasValue)
                {
                    return @from;
                }

                var tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();
                var endOfDay = tz.AtStartOfDay(InstantProvider.Now.InZone(tz).Date.PlusDays(1));
                return endOfDay.ToInstant();
            }
            set
            {
                to = value;
                FilterChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [Parameter]
        public Guid UserId
        {
            get => userId;
            set
            {
                userId = value;
                FilterChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private event EventHandler FilterChanged;

        public TimeTrackingEntryReportBase()
        {
            FilterChanged += async (sender, args) => { await RefreshAsync(); };
        }

        [Inject] public ITimeTrackingService TimeTrackingService { get; set; }
        [Inject] public IInstant InstantProvider { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RefreshAsync();
            }
        }

        public TimeTrackingEntryReportVm TimeTrackingEntryReportVm { get; set; }

        public async Task RefreshAsync()
        {
            if (Guid.Empty.Equals(userId))
            {
                return;
            }

            TimeTrackingEntryReportVm = await TimeTrackingService.ReportAsync(UserId, From!.Value, To!.Value);
            StateHasChanged();
        }
    }
}