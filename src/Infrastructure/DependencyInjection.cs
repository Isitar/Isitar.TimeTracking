namespace Isitar.TimeTracking.Infrastructure
{
    using Application.Common.Interfaces;
    using Common;
    using Instant;
    using Microsoft.Extensions.DependencyInjection;
    using StorageProvider;
    using Identity;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IInstant, SystemClockInstant>();
            services.AddTransient<IStorageProvider, FileStorageProvider>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.Addiden
            return services;
        }
    }
}