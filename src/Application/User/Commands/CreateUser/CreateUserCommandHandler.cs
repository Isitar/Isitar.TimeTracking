namespace Isitar.TimeTracking.Application.User.Commands.CreateUserCommand
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Interfaces;
    using CreateUser;
    using Domain.Entities;
    using MediatR;

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly ITimeTrackingDbContext dbContext;
        private readonly IIdentityService identityService;

        public CreateUserCommandHandler(ITimeTrackingDbContext dbContext, IIdentityService identityService)
        {
            this.dbContext = dbContext;
            this.identityService = identityService;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await dbContext.Users.AddAsync(new User
                {
                    Id = request.Id,
                    Acronym = request.Acronym,
                    Name = request.Name,
                    Email = request.Email,
                    Locale = request.Locale,
                }, cancellationToken);
                var createUserResult = await identityService.CreateUserAsync(request.Id, request.Acronym, request.Email, request.Password);
                if (!createUserResult.Successful)
                {
                    throw new Exception(createUserResult.ErrorsCompact());
                }

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                try
                {
                    var user = await dbContext.Users.FindAsync(request.Id);
                    if (null != user)
                    {
                        dbContext.Users.Remove(user);
                    }

                    await dbContext.SaveChangesAsync(cancellationToken);
                }
                catch
                {
                    // ignored
                }

                try
                {
                    await identityService.DeleteUserAsync(request.Id);
                }
                catch
                {
                    // ignored
                }

                throw;
            }


            return Unit.Value;
        }
    }
}