namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Queries.TimeTrackingEntryReport
{
    using FluentValidation;

    public class TimeTrackingEntryReportQueryValidator : AbstractValidator<TimeTrackingEntryReportQuery>
    {
        public TimeTrackingEntryReportQueryValidator()
        {
            RuleFor(x => x.From)
                .LessThan(x => x.To);
        }
    }
}