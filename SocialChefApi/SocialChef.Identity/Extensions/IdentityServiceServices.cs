using IdentityServer4.AspNetIdentity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialChef.Identity.Models;

namespace SocialChef.Identity.Extensions
{
    public static class IdentityServiceServices
    {
        public static void AddIdentityServer(this IServiceCollection services, IConfiguration configuration, string connectionString, string migrationsAssembly)
        {
            services.AddIdentityServer(options =>
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
                .AddAspNetIdentity<User>()
                .AddProfileService<ProfileService<User>>()
                .AddSigningCredentials(configuration);
        }
    }
}