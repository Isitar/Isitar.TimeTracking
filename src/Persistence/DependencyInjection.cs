namespace Isitar.TimeTracking.Persistence
{
    using System.Reflection;
    using Application.Common.Interfaces;
    using Infrastructure.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using StorageProvider;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TimeTrackingDbContext>(options =>
                options
                    .UseLazyLoadingProxies()
                    .UseNpgsql(configuration.GetConnectionString("TimeTrackingDbConnection"),
                        o => o.UseNodaTime()
                    )
            );

            services.AddDbContext<AppIdentityDbContext>(
                options =>
                    options
                        .UseLazyLoadingProxies()
                        .UseNpgsql(configuration.GetConnectionString("AppIdentityDbConnection"),
                            o => o
                                .MigrationsAssembly(Assembly.GetAssembly(typeof(DependencyInjection))!.FullName)
                                .UseNodaTime()
                        )
            );

            services.AddScoped<ITimeTrackingDbContext, TimeTrackingDbContext>();


            var fsc = new FileStorageConfig();
            configuration.Bind(nameof(FileStorageConfig), fsc);
            services.AddSingleton(fsc);
            services.AddTransient<IStorageProvider, FileStorageProvider>();
            return services;
        }
    }
}