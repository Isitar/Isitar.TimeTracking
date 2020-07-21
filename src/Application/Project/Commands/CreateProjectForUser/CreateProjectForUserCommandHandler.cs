namespace Isitar.TimeTracking.Application.Project.Commands.CreateProject
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using Domain.Entities;
    using MediatR;

    public class CreateProjectForUserCommandHandler : IRequestHandler<CreateProjectForUserCommand>
    {
        private readonly ITimeTrackingDbContext dbContext;
        private readonly IStorageProvider storageProvider;

        public CreateProjectForUserCommandHandler(ITimeTrackingDbContext dbContext, IStorageProvider storageProvider)
        {
            this.dbContext = dbContext;
            this.storageProvider = storageProvider;
        }

        public async Task<Unit> Handle(CreateProjectForUserCommand request, CancellationToken cancellationToken)
        {
            string imagePath = null;
            if (null != request.ImageStream)
            {
                var storageResult = await storageProvider.SaveAsync(request.ImageStream, request.ImageStreamFilename);
                if (storageResult.Successful)
                {
                    imagePath = storageResult.Data;
                }
                else
                {
                    throw new Exception(storageResult.ErrorsCompact());
                }
            }

            await dbContext.Projects.AddAsync(new Project
            {
                Id = request.Id,
                Name = request.Name,
                UserId = request.UserId,
                ImagePath = imagePath,
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}