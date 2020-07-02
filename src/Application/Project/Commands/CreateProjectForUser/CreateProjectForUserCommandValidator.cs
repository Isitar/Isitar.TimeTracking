namespace Isitar.TimeTracking.Application.Project.Commands.CreateProject
{
    using System.Linq;
    using Common.Interfaces;
    using FluentValidation;
    using global::Common.Resources;

    public class CreateProjectForUserCommandValidator : AbstractValidator<CreateProjectForUserCommand>
    {
        public CreateProjectForUserCommandValidator(ITimeTrackingDbContext dbContext)
        {
            RuleFor(x => x.UserId)
                .Must(userId => dbContext.Users.Any(u => u.Id.Equals(userId)))
                .WithMessage(cmd => Translation.NotFoundException.Replace("{name}", Translation.User).Replace("{key}", cmd.UserId.ToString()));

            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.ImageStreamFilename)
                .NotEmpty()
                .When(cmd => null != cmd.ImageStream);
            
            
        }
    }
}