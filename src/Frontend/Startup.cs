namespace Isitar.TimeTracking.Frontend
{
    using System;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using Blazored.LocalStorage;
    using Configs;
    using Data;
    using global::Common;
    using Infrastructure;
    using Infrastructure.Instant;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var apiConfig = new ApiConfig();
            Configuration.Bind("Api", apiConfig);
            services.AddSingleton(apiConfig);
            
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddHttpClient<IAuthService, AuthService>(cfg =>
            {
                cfg.BaseAddress = new Uri(apiConfig.BaseUrl);
            });
            services.AddScoped<IProjectService, ProjectService>();    
            services.AddHttpClient<IGenericService, GenericService>(cfg =>
            {
                cfg.BaseAddress = new Uri(apiConfig.BaseUrl);
            });

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            services.AddSingleton(jsonSerializerOptions);

            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();


            services.AddBlazoredLocalStorage();
            services.AddTransient<IInstant, SystemClockInstant>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}