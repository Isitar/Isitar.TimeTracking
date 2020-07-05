namespace Isitar.TimeTracking.Application.User.Commands.CreateUserCommand
{
    using CreateUser;
    using FluentValidation;

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Acronym).NotEmpty().MaximumLength(10);
            RuleFor(x => x.Locale).NotEmpty().Matches("[a-z]{2}-[A-Z]{2}$");

            RuleFor(x => x.Password).NotEmpty();
        }
    }
}