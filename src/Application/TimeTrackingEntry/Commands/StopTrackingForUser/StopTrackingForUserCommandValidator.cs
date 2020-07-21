namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Commands.StopTrackingForUser
{
    using System.Linq;
    using Common.Interfaces;
    using FluentValidation;
    using global::Common.Resources;

    public class StopTrackingForUserCommandValidator : AbstractValidator<StopTrackingForUserCommand>
    {
        public StopTrackingForUserCommandValidator(ITimeTrackingDbContext dbContext)
        {
            RuleFor(x => x.UserId)
                .Must(userId => dbContext.Users.Any(u => u.Id.Equals(userId)))
                .WithMessage(cmd => Translation.NotFoundException.Replace("{name}", Translation.User).Replace("{key}", cmd.UserId.ToString()));
        }
    }
}