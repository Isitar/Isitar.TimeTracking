namespace Common
{
    using NodaTime;

    public interface IInstant
    {
        Instant Now { get; }
    }
}