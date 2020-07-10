namespace Isitar.TimeTracking.Application.Project.Queries.ProjectImage
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using global::Common.Resources;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class ProjectImageQueryHandler : IRequestHandler<ProjectImageQuery, ProjectImageDto>
    {
        private readonly ITimeTrackingDbContext dbContext;
        private readonly IStorageProvider storageProvider;

        public ProjectImageQueryHandler(ITimeTrackingDbContext dbContext, IStorageProvider storageProvider)
        {
            this.dbContext = dbContext;
            this.storageProvider = storageProvider;
        }

        public async Task<ProjectImageDto> Handle(ProjectImageQuery request, CancellationToken cancellationToken)
        {
            var imagePath = await dbContext.Projects
                .Where(p => p.Id.Equals(request.Id))
                .Select(p => p.ImagePath)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (null == imagePath)
            {
                throw new NotFoundException(Translation.ProjectImage, request.Id);
            }

            var storageResult = await storageProvider.OpenAsync(imagePath);
            if (!storageResult.Successful)
            {
                throw new Exception(storageResult.ErrorsCompact());
            }

            return new ProjectImageDto
            {
                Data = storageResult.Data.content,
                Filename = storageResult.Data.filename,
            };
        }
    }
}