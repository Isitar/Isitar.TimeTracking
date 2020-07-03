namespace Isitar.TimeTracking.Api
{
    using Application;
    using Application.Common.Interfaces;
    using Infrastructure;
    using Infrastructure.Identity;
    using Installers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Middlewares;
    using Persistence;
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
            services.AddControllers();
            services.AddInfrastructure(Configuration);
            services.AddPersistence(Configuration);
            services.AddApplication();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddSwagger();

            services.AddCors(options => { options.AddPolicy("AllowAll", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("Token-Expired", "content-disposition"); }); });
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

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware(typeof(LanguageMiddleware));
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}