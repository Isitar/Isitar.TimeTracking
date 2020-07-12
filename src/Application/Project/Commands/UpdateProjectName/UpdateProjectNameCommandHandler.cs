namespace Isitar.TimeTracking.Application.Project.Commands.UpdateProjectName
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using global::Common.Resources;
    using MediatR;

    public class UpdateProjectNameCommandHandler : IRequestHandler<UpdateProjectNameCommand>
    {
        private readonly ITimeTrackingDbContext dbContext;

        public UpdateProjectNameCommandHandler(ITimeTrackingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(UpdateProjectNameCommand request, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.FindAsync(request.Id);
            if (null == project)
            {
                throw new NotFoundException(Translation.Project, request.Id);
            }

            project.Name = request.Name;
            dbContext.Projects.Update(project);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}