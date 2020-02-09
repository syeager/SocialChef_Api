using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LittleByte.Asp.Application;
using LittleByte.Asp.Configuration;
using LittleByte.Asp.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
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
            }

            app.UseCors(policy => policy.AllowAnyOrigin());
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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var identityOptions = configuration.GetSection<IdentityOptions>(IdentityOptions.Key);

                o.Authority = identityOptions.Address;
                o.Audience = "api1";
                o.RequireHttpsMetadata = false;
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