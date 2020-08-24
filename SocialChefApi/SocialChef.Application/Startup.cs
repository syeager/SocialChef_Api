using System.Text.Json;
using JetBrains.Annotations;
using LittleByte.Asp.Application;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;

namespace SocialChef.Application
{
    public class Startup
    {
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration configuration;

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            if(!environment.IsDevelopment())
            {
                services.ConfigureNonBreakingSameSiteCookies();
            }

            services.AddHealthChecks();
            services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; });
            services.AddSwaggerDocument(settings => { settings.Title = "Social Chef API"; });
            Domain.Startup.ConfigureServices(services, configuration);
            AddLogging();
            AddIdentity();

            void AddLogging()
            {
                // TODO: Add key.
                services.AddLogging(builder => { builder.AddApplicationInsights(""); });
                services.AddApplicationInsightsTelemetry();
            }

            void AddIdentity()
            {
                // TODO: Require confirmation
                services.AddDefaultIdentity<Domain.Relational.UserDao>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<Domain.Relational.SqlDbContext>();

                services.AddIdentityServer()
                    .AddApiAuthorization<Domain.Relational.UserDao, Domain.Relational.SqlDbContext>();

                services.AddAuthentication()
                    .AddIdentityServerJwt();
            }
        }

        [UsedImplicitly]
#pragma warning disable CA1822
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                IdentityModelEventSource.ShowPII = true;
                app.UseCookiePolicy(new CookiePolicyOptions
                {
                    Secure = CookieSecurePolicy.None,
                    MinimumSameSitePolicy = SameSiteMode.Lax,
                });
            }
            else
            {
                app.UseCookiePolicy();
                app.UseHttpsRedirection();
            }

            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseHealthChecks();
            app.UseRouting();
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseHttpExceptions();
            app.UseModelValidationExceptions();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}