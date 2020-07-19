namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingEntryRapport
{
    using FluentValidation;

    public class TimeTrackingEntryRapportQueryValidator : AbstractValidator<TimeTrackingEntryRapportQuery>
    {
        public TimeTrackingEntryRapportQueryValidator()
        {
            RuleFor(x => x.From)
                .LessThan(x => x.To);
        }
    }
}