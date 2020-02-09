using LittleByte.Asp.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocialChef.Identity.ConfigOptions;
using SocialChef.Identity.Data;
using SocialChef.Identity.Models;

namespace SocialChef.Identity
{
    public class Startup
    {
        private IWebHostEnvironment Environment { get; }
        private IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            ConfigureWindowsIisOptions(services);

            var (connectionString, migrationsAssembly) = GetDatabaseOptions();

            ConfigureDatabase(services, connectionString, migrationsAssembly);

            ConfigureIdentityServer(services, connectionString, migrationsAssembly);

            AddGoogleAuthentication(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            if(Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }

        private (string connectionString, string migrationsAssembly) GetDatabaseOptions()
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = typeof(Startup).Assembly.FullName;
            return (connectionString, migrationsAssembly);
        }

        private static void ConfigureWindowsIisOptions(IServiceCollection services)
        {
            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });
        }

        private static void ConfigureDatabase(IServiceCollection services, string connectionString, string migrationsAssembly)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly(migrationsAssembly)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        private static void ConfigureIdentityServer(IServiceCollection services, string connectionString, string migrationsAssembly)
        {
            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddConfigurationStore(options => { options.ConfigureDbContext = b => b.UseSqlServer(connectionString, x => x.MigrationsAssembly(migrationsAssembly)); })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString, x => x.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                })
                .AddAspNetIdentity<ApplicationUser>();

            // TODO: What are you?
            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();
        }

        private static void SeedDatabase()
        {
            //using(var scope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            //    SeedData.EnsureSeedData(context, scope);
            //}
        }

        private void AddGoogleAuthentication(IServiceCollection services)
        {
            var googleAuthOptions = Configuration.GetSection<GoogleAuthOptions>(GoogleAuthOptions.Key);
            if(googleAuthOptions == null)
            {
                return;
            }

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = googleAuthOptions.ClientID;
                    options.ClientSecret = googleAuthOptions.ClientSecret;
                });
        }
    }
}