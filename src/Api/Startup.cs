using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api
{
    using Installers;
    using Isitar.TimeTracking.Application;
    using Isitar.TimeTracking.Application.Common.Interfaces;
    using Isitar.TimeTracking.Infrastructure;
    using Isitar.TimeTracking.Infrastructure.Identity;
    using Isitar.TimeTracking.Infrastructure.StorageProvider;
    using Isitar.TimeTracking.Persistence;
    using Microsoft.EntityFrameworkCore;
    using Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var fsc = new FileStorageConfig();
            Configuration.Bind(nameof(FileStorageConfig), fsc);
            services.AddSingleton<FileStorageConfig>(fsc);
            
            services.AddControllers();
            services.AddInfrastructure();
            services.AddPersistence(Configuration);
            services.AddApplication();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddSwagger();
            
            
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ITimeTrackingDbContext dbContext, AppIdentityDbContext appIdentityDbContext)
        {
            dbContext.Database.Migrate();
            appIdentityDbContext.Database.Migrate();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerWithUi();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}