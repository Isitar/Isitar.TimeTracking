namespace Isitar.TimeTracking.Application
{
    using System.Linq;
    using System.Reflection;
    using AutoMapper;
    using Common.Authorization;
    using Common.Behaviors;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestAuthorizationBehavior<,>));

            // add authorizers
            foreach (var type in Assembly.GetExecutingAssembly()
                .DefinedTypes.Where(x => x.IsClass
                                         && !x.IsAbstract
                                         && x.GetInterfaces().Any(i =>
                                             i.IsGenericType
                                             && i.GetGenericTypeDefinition() == typeof(IAuthorizer<>))
                ))
            {
                foreach (var implementedInterface in type.ImplementedInterfaces)
                {
                    services.AddTransient(implementedInterface, type);
                }
            }

            return services;
        }
    }
}