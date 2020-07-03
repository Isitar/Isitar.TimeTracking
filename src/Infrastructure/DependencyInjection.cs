namespace Isitar.TimeTracking.Infrastructure
{
    using System;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Common;
    using Instant;
    using Microsoft.Extensions.DependencyInjection;
    using StorageProvider;
    using Identity;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var fsc = new FileStorageConfig();
            configuration.Bind(nameof(FileStorageConfig), fsc);
            services.AddSingleton(fsc);

            services.AddTransient<IInstant, SystemClockInstant>();
            services.AddTransient<IStorageProvider, FileStorageProvider>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddIdentity<AppUser, AppRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredLength = 8;

                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-._@";
                })
                .AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddDbContext<AppIdentityDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("AppIdentityDbConnection")));
            
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(JwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero,
            };
            services.AddAuthentication(config =>
                {
                    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddCookie(options => { options.SlidingExpiration = true; })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }

                            return Task.CompletedTask;
                        },
                    };
                });
            
            // add policy for permissions
            services.AddAuthorization(options =>
            {
                foreach (var prop in typeof(ClaimPermission).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
                {
                    var propertyValue = prop.GetValue(null)?.ToString();
                    options.AddPolicy(propertyValue, policy => policy.RequireClaim(CustomClaimTypes.PermissionClaimType, propertyValue));
                }
            });
            return services;
        }
    }
}