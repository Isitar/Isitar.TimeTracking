namespace Isitar.TimeTracking.Application.Project.Commands.DeleteProject
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using global::Common.Resources;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand>
    {
        private readonly ITimeTrackingDbContext dbContext;

        public DeleteProjectCommandHandler(ITimeTrackingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var proj = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken: cancellationToken);
            if (null == proj)
            {
                throw new NotFoundException(Translation.Project, request.Id);
            }

            dbContext.Projects.Remove(proj);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}