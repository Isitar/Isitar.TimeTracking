namespace Isitar.TimeTracking.Application.User.Queries.UserDetail
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

    public class UserDetailQueryHandler : IRequestHandler<UserDetailQuery, UserDetailVm>
    {
        private readonly ITimeTrackingDbContext dbContext;
        private readonly IMapper mapper;

        public UserDetailQueryHandler(ITimeTrackingDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<UserDetailVm> Handle(UserDetailQuery request, CancellationToken cancellationToken)
        {
            
            var vm = await dbContext.Users
                .Where(u => u.Id.Equals(request.Id))
                .Include(u => u.CreatedBy)
                .Include(u => u.UpdatedBy)
                .Include(u => u.AuditTrailEntries)
                .ProjectTo<UserDetailVm>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
            if (null == vm)
            {
                throw new NotFoundException(Translation.User, request.Id);
            }

            return vm;
        }
    }
}