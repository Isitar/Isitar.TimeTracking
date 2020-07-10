namespace Isitar.TimeTracking.Application.Project.Queries.ProjectDetail
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Exceptions;
    using Common.Interfaces;
    using global::Common.Resources;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class ProjectDetailQueryHandler : IRequestHandler<ProjectDetailQuery, ProjectDetailVm>
    {
        private readonly ITimeTrackingDbContext dbContext;
        private readonly IMapper mapper;
        private readonly IStorageProvider storageProvider;


        public ProjectDetailQueryHandler(ITimeTrackingDbContext dbContext, IMapper mapper, IStorageProvider storageProvider)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.storageProvider = storageProvider;
        }

        public async Task<ProjectDetailVm> Handle(ProjectDetailQuery request, CancellationToken cancellationToken)
        {
            var vm = await dbContext.Projects
                .Where(p => p.Id.Equals(request.Id))
                .ProjectTo<ProjectDetailVm>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
            if (null == vm)
            {
                throw new NotFoundException(Translation.Project, request.Id);
            }

            return vm;
        }
    }
}