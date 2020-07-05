namespace Isitar.TimeTracking.Application.Setup.Commands.InitializeApplication
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Enums;
    using Common.Interfaces;
    using MediatR;
    using User.Commands.CreateUser;

    public class InitializeApplicationCommandHandler : IRequestHandler<InitializeApplicationCommand>
    {
        private readonly IMediator mediator;
        private readonly ITimeTrackingDbContext dbContext;
        private readonly IIdentityService identityService;

        public InitializeApplicationCommandHandler(IMediator mediator, ITimeTrackingDbContext dbContext, IIdentityService identityService)
        {
            this.mediator = mediator;
            this.dbContext = dbContext;
            this.identityService = identityService;
        }
        
        public async Task<Unit> Handle(InitializeApplicationCommand request, CancellationToken cancellationToken)
        {
            if (dbContext.Users.Any())
            {
                return Unit.Value;
            }

            var id = Guid.NewGuid();
            await mediator.Send(new CreateUserCommand
            {
                Acronym = "lpa",
                Id = id,
                Locale = "de-CH",
                Name = "Pascal Lüscher",
                Email = "luescherpascal@gmail.com",
                Password = "Tester1.",
            }, cancellationToken);

            await identityService.AssignRoleAsync(id, RoleNames.Admin);
            
            return Unit.Value;
        }
    }
}