using LittleByte.Asp.Application;
using LittleByte.Asp.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using SocialChef.Identity.Contexts;
using SocialChef.Identity.Extensions;

namespace SocialChef.Identity
{
    public class Startup
    {
        private readonly IWebHostEnvironment environment;
        private readonly IConfiguration configuration;

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.environment = environment;
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if(!environment.IsDevelopment())
            {
                services.ConfigureNonBreakingSameSiteCookies();
            }

            services.AddHealthChecks().AddDbContextCheck<UserDbContext>("SqlDB");
            services.AddControllersWithViews();
            services.AddGoogleAuthentication(configuration);

            var (connectionString, migrationsAssembly) = GetDatabaseOptions();
            services.ConfigureDatabase(connectionString);
            services.AddIdentityServer(configuration, connectionString, migrationsAssembly);
            services.SeedDatabase();
        }

        public void Configure(IApplicationBuilder app)
        {
            if(environment.IsDevelopment())
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
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }

        private (string connectionString, string migrationsAssembly) GetDatabaseOptions()
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).Assembly.FullName;
            return (connectionString, migrationsAssembly);
        }
    }
}