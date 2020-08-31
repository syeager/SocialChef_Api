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
    public class ClientSetupService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public ClientSetupService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();

            await EnsureDatabaseCreated(scope);
            await EnsureClientsCreated(scope);
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private static async Task EnsureDatabaseCreated(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetRequiredService<SqlDbContext>();
            await context.Database.EnsureCreatedAsync();
        }

        private static async Task EnsureClientsCreated(IServiceScope scope)
        {
            var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictEntityFrameworkCoreApplication>>();

            foreach(var client in Clients.Get())
            {
                if(await manager.FindByClientIdAsync(client.ClientId!) == null)
                {
                    await manager.CreateAsync(client);
                }
            }
        }
    }
}