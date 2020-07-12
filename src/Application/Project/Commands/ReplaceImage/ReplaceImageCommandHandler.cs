namespace Isitar.TimeTracking.Application.Project.Commands.ReplaceImage
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using global::Common.Resources;
    using MediatR;

    public class ReplaceImageCommandHandler : IRequestHandler<ReplaceImageCommand>
    {
        private readonly IStorageProvider storageProvider;
        private readonly ITimeTrackingDbContext dbContext;

        public ReplaceImageCommandHandler(IStorageProvider storageProvider, ITimeTrackingDbContext dbContext)
        {
            this.storageProvider = storageProvider;
            this.dbContext = dbContext;
        }

        public async Task<Unit> Handle(ReplaceImageCommand request, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.FindAsync(request.Id);
            if (null == project)
            {
                throw new NotFoundException(Translation.Project, request.Id);
            }

            var oldImagePath = project.ImagePath;

            var storageResult = await storageProvider.SaveAsync(request.ImageStream, request.ImageStreamFilename);
            if (storageResult.Successful)
            {
                project.ImagePath = storageResult.Data;
            }
            else
            {
                throw new Exception(storageResult.ErrorsCompact());
            }

            dbContext.Projects.Update(project);
            await dbContext.SaveChangesAsync(cancellationToken);


            if (!string.IsNullOrWhiteSpace(oldImagePath))
            {
                await storageProvider.DeleteAsync(oldImagePath);
            }

            return Unit.Value;
        }
    }
}