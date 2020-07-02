namespace Isitar.TimeTracking.Application.Project.Queries.ProjectList
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class ProjectListQueryHandler : IRequestHandler<ProjectListQuery, ProjectListVm>
    {
        private readonly ITimeTrackingDbContext dbContext;
        private readonly IMapper mapper;

        public ProjectListQueryHandler(ITimeTrackingDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<ProjectListVm> Handle(ProjectListQuery request, CancellationToken cancellationToken)
        {
            var query = dbContext.Projects.AsQueryable();

            if (request.UserFilter.Any())
            {
                query = query.Where(project => request.UserFilter.Contains(project.UserId));
            }

            var projects = await query
                .ProjectTo<ProjectSlimDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new ProjectListVm()
            {
                Projects = projects
            };
        }
    }
}