namespace Isitar.TimeTracking.Frontend.Components.TimeTrackingEntry
{
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.TimeTrackingEntry.Queries.TimeTrackingEntryDetail;
    using global::Common;
    using Microsoft.AspNetCore.Components;

    public class TimeTrackingEntryCardBase : ComponentBase
    {
        [Parameter] public TimeTrackingEntryDetailVm TimeTrackingEntryDetail { get; set; }

        private Timer t;

        [Inject] public IInstant Instant { get; set; }

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

    }
}