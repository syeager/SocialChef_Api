using System;
using System.Threading;
using System.Threading.Tasks;
using LittleByte.Asp.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SocialChef.Persistence
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureCosmos(services, configuration);
        }

        private static void ConfigureCosmos(IServiceCollection services, IConfiguration configuration)
        {
            var options = ConfigureOptions(services, configuration);

            AddHealthCheck(services, options);

            services.AddDbContext<CosmosContext>();

            CreateCosmos(options).GetAwaiter();
        }

        private static CosmosOptions ConfigureOptions(IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(CosmosOptions.OptionsKey);
            services.Configure<CosmosOptions>(section);

            var options = section.Get<CosmosOptions>();
            return options;
        }

        private static void AddHealthCheck(IServiceCollection services, CosmosOptions options)
        {
            services.AddHealthChecks().AddDbContextCheck<CosmosContext>("CosmosDB", customTestQuery: HealthCheck);

            Task<bool> HealthCheck(CosmosContext context, CancellationToken token)
            {
                return context.CanConnectAsync(options.DatabaseName, options.ContainerName, token);
            }
        }

        private static async Task CreateCosmos(CosmosOptions options)
        {
            try
            {
                await using var context = new CosmosContext(new OptionsWrapper<CosmosOptions>(options));
                var response = await context.Database.GetCosmosClient().CreateDatabaseIfNotExistsAsync(options.DatabaseName);
                await response.Database.CreateContainerIfNotExistsAsync(options.ContainerName, "/_");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}