namespace Isitar.TimeTracking.Frontend.Components.TimeTrackingEntry
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.TimeTrackingEntry.Queries.TimeTrackingEntryDetail;
    using Frontend.Common.Authentication;
    using global::Common;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using Services;

    public class TimeTrackingEntryCardBase : ComponentBase
    {
        [Parameter] public TimeTrackingEntryDetailVm TimeTrackingEntryDetail { get; set; }
        [Parameter] public EventCallback<Guid> StoppedWorking { get; set; }

        private Timer t;

        [Inject] public IInstant Instant { get; set; }

        [Inject] public IUserService UserService { get; set; }
        [Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        public string TimePassed { get; private set; } = "test";

        protected override void OnInitialized()
        {
            t = new Timer(_ =>
            {
                var delta = Instant.Now.Minus(TimeTrackingEntryDetail.From);
                var res = string.Empty;
                if (delta.Days > 0)
                {
                    res += $"{delta.Days} days, ";
                }

                TimePassed = res + delta.ToString("hh:mm:ss", new CultureInfo("de-CH"));
                InvokeAsync(StateHasChanged);
            }, null, 1000, 1000);
        }

        protected async Task StopWorking()
        {
            var userId = await AuthenticationStateProvider.UserIdAsync();
            await UserService.StopTrackingAsync(userId);
            await StoppedWorking.InvokeAsync(userId);
        }
    }
}