using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using JetBrains.Annotations;
using LittleByte.Asp.Application;
using LittleByte.Asp.Configuration;
using LittleByte.Asp.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using SocialChef.Domain.Recipes;

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
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            if(!environment.IsDevelopment())
            {
                services.ConfigureNonBreakingSameSiteCookies();
            }

            services.AddHealthChecks();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
            services.AddSwaggerDocument();

            services.AddLogging(builder => { builder.AddApplicationInsights(""); });
            services.AddApplicationInsightsTelemetry();

            AddAuthentication(services);

            Domain.Startup.ConfigureServices(services, configuration);
        }

        [UsedImplicitly]
#pragma warning disable CA1822
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
            app.UseHealthChecks("/health", new HealthCheckOptions {ResponseWriter = WriteHealthResponse});
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpExceptions();
            app.UseModelValidationExceptions();

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void AddAuthentication(IServiceCollection services)
        {
            var identityOptions = configuration.GetSection<IdentityOptions>(IdentityOptions.Key);

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = identityOptions.Address;
                    options.ApiName = "api1";
                    options.SupportedTokens = SupportedTokens.Jwt;
                    // TODO: Remove if not develop
                    options.RequireHttpsMetadata = false;
                });
        }

        private static Task WriteHealthResponse(HttpContext httpContext, HealthReport report)
        {
            httpContext.Response.ContentType = "application/json";
            var responseJson = JsonSerializer.Serialize(report, new JsonSerializerOptions
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter(),
                    new TimespanConverter(@"ss\:fff"),
                }
            });
            return httpContext.Response.WriteAsync(responseJson);
        }
    }
}