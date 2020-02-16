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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using SocialChef.Application.ConfigOptions;

namespace SocialChef.Application
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddControllers();
            services.AddSwaggerDocument();

            AddAuthentication(services);

            Business.Startup.ConfigureServices(services, configuration);
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseHttpsRedirection();
            app.UseHealthChecks("/health", new HealthCheckOptions {ResponseWriter = WriteHealthResponse});
            app.UseHttpExceptions();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

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