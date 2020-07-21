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
        private readonly IIdentityInitializer identityInitializer;

        public InitializeApplicationCommandHandler(IMediator mediator,
            ITimeTrackingDbContext dbContext,
            IIdentityService identityService,
            IIdentityInitializer identityInitializer)
        {
            this.mediator = mediator;
            this.dbContext = dbContext;
            this.identityService = identityService;
            this.identityInitializer = identityInitializer;
        }

        public async Task<Unit> Handle(InitializeApplicationCommand request, CancellationToken cancellationToken)
        {
            if (dbContext.Users.Any())
            {
                return Unit.Value;
            }

            await identityInitializer.Initialize();

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