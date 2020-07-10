namespace Isitar.TimeTracking.Application.Common.Behaviors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Authorization;
    using Exceptions;
    using FluentValidation;
    using MediatR;

    public class RequestAuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IAuthorizer<TRequest>> authorizers;

        public RequestAuthorizationBehavior(IEnumerable<IAuthorizer<TRequest>> authorizers)
        {
            this.authorizers = authorizers;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            foreach (var authorizer in authorizers)
            {
                if (!await authorizer.AuthorizeAsync(request))
                {
                    throw new UnauthorizedException();
                } 
            }
            return await next();
        }
    }
}