using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using SocialChef.Domain.Identity.Config;
using SocialChef.Domain.Relational;

namespace SocialChef.Domain.Identity
{
    public class OidcBootService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public OidcBootService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var serviceScope = serviceProvider.CreateScope();

            await EnsureDatabaseCreated(serviceScope);
            await EnsureClientsCreated(serviceScope);
            await EnsureScopesCreated(serviceScope);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private static async Task EnsureDatabaseCreated(IServiceScope serviceScope)
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<SqlDbContext>();
            await context.Database.EnsureCreatedAsync();
        }

        private static async Task EnsureClientsCreated(IServiceScope serviceScope)
        {
            var manager = serviceScope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictEntityFrameworkCoreApplication>>();

            foreach(var client in Clients.Get())
            {
                if(await manager.FindByClientIdAsync(client.ClientId!) == null)
                {
                    await manager.CreateAsync(client);
                }
            }
        }

        private static async Task EnsureScopesCreated(IServiceScope serviceScope)
        {
            var manager = serviceScope.ServiceProvider.GetRequiredService<OpenIddictScopeManager<OpenIddictEntityFrameworkCoreScope>>();

            foreach(var scope in Scopes.Get())
            {
                if(await manager.FindByNameAsync(scope.Name!) == null)
                {
                    await manager.CreateAsync(scope);
                }
            }
        }
    }
}