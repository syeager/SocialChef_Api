using System;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialChef.Identity.Contexts;
using SocialChef.Identity.Models;

namespace SocialChef.Identity.Extensions
{
    public static class DatabaseServices
    {
        public static void ConfigureDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(connectionString, x => x.MigrationsAssembly(typeof(UserDbContext).Assembly.FullName)));

            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void SeedDatabase(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();

            var configurationDbContext = scope.ServiceProvider.GetService<ConfigurationDbContext>();

            SeedData.EnsureSeedData(configurationDbContext);
        }
    }
}