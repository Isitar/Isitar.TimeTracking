namespace Isitar.TimeTracking.Application.User.Queries.UserLocale
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Exceptions;
    using Common.Interfaces;
    using global::Common.Resources;
    using MediatR;

    public class UserLocaleQueryHandler : IRequestHandler<UserLocaleQuery, UserLocaleVm>
    {
        private readonly ITimeTrackingDbContext dbContext;
        private readonly IMapper mapper;

        public UserLocaleQueryHandler(ITimeTrackingDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<UserLocaleVm> Handle(UserLocaleQuery request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FindAsync(request.Id);
            if (null == user)
            {
                throw new NotFoundException(Translation.User, request.Id);
            }

            return mapper.Map<UserLocaleVm>(user);
        }
    }
}