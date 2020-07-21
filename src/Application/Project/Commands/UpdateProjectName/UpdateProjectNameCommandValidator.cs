namespace Isitar.TimeTracking.Application.Project.Commands.UpdateProjectName
{
    using System.Linq;
    using Common.Interfaces;
    using FluentValidation;
    using global::Common.Resources;

    public class UpdateProjectNameCommandValidator : AbstractValidator<UpdateProjectNameCommand>
    {
        public UpdateProjectNameCommandValidator(ITimeTrackingDbContext dbContext)
        {
            RuleFor(x => x.Id).Must(projectId => dbContext.Projects.Any(p => p.Id.Equals(projectId)))
                .WithMessage(cmd => Translation.NotFoundException.Replace("{name}", Translation.Project).Replace("{key}", cmd.Id.ToString()));

            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        }
    }
}