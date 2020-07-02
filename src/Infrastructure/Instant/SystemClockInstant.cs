namespace Isitar.TimeTracking.Infrastructure.Instant
{
    using Common;
    using NodaTime;

    public class SystemClockInstant : IInstant
    {
        public Instant Now => SystemClock.Instance.GetCurrentInstant();
    }
}