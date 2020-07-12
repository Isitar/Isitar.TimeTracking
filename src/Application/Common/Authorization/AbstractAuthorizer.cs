namespace Isitar.TimeTracking.Application.Common.Authorization
{
    using System.Threading.Tasks;
    using Interfaces;
    using MediatR;

    public abstract class AbstractAuthorizer<T> : IAuthorizer<T>
    {
        protected readonly ICurrentUserService CurrentUserService;
        protected readonly IMediator Mediator;

        public AbstractAuthorizer(ICurrentUserService currentUserService, IMediator mediator)
        {
            CurrentUserService = currentUserService;
            Mediator = mediator;
        }

        public abstract Task<bool> AuthorizeAsync(T request);
    }
}