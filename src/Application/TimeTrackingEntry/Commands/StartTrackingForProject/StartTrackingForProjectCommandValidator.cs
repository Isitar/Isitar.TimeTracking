namespace Isitar.TimeTracking.Application.TimeTrackingEntry.Commands.StartTrackingForProject
{
    using System;
    using System.Linq;
    using Common.Interfaces;
    using Domain.Entities;
    using FluentValidation;
    using FluentValidation.Resources;
    using global::Common.Resources;
    using MediatR;

    public class StartTrackingForProjectCommandValidator : AbstractValidator<StartTrackingForProjectCommand>
    {
        public StartTrackingForProjectCommandValidator(ITimeTrackingDbContext dbContext)
        {
            
            RuleFor(x => x.UserId)
                .Must(userId => dbContext.Users.Any(u => u.Id.Equals(userId)))
                .WithMessage(cmd => Translation.NotFoundException.Replace("{name}", Translation.User).Replace("{key}", cmd.UserId.ToString()));
            
            RuleFor(x => x.ProjectId)
                .Must(projectId => dbContext.Projects.Any(p => p.Id.Equals(projectId)))
                .WithMessage(cmd => Translation.NotFoundException.Replace("{name}", Translation.Project).Replace("{key}", cmd.ProjectId.ToString()));
            
                    
            RuleFor(x => new {x.UserId, x.ProjectId})
                .Must(prop => dbContext.Projects.Any(p => p.Id.Equals(prop.ProjectId) && p.UserId.Equals(prop.UserId)))
                .WithMessage(cmd => Translation.NotFoundException.Replace("{name}", Translation.Project).Replace("{key}", cmd.ProjectId.ToString()));
            
        }
    }
}