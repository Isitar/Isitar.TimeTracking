namespace Isitar.TimeTracking.Infrastructure
{
    using Application.Common.Interfaces;
    using Common;
    using Instant;
    using Microsoft.Extensions.DependencyInjection;
    using StorageProvider;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IInstant, SystemClockInstant>();
            services.AddTransient<IStorageProvider, FileStorageProvider>();
            return services;
        }
    }
}