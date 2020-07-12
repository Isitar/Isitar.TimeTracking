namespace Isitar.TimeTracking.Application.Project.Commands.ReplaceImage
{
    using FluentValidation;

    public class ReplaceImageCommandValidator : AbstractValidator<ReplaceImageCommand>
    {
        public ReplaceImageCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();
            RuleFor(x => x.ImageStream)
                .NotNull();
            RuleFor(x => x.ImageStreamFilename)
                .NotEmpty();
        }
    }
}