using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LittleByte.Asp.Database;
using LittleByte.Asp.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialChef.Business.ConfigOptions;
using SocialChef.Business.Document.Contexts;
using SocialChef.Business.Document.Options;
using SocialChef.Business.Relational.Contexts;
using SocialChef.Business.Services;

namespace SocialChef.Business
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            AddOptions(services, configuration);
            AddServices(services);
            AddDataStores(services, configuration);
        }

        private static void AddOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureOptions<IdentityOptions>(configuration, IdentityOptions.Key);
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<HttpClient>();

            services.AddTransient<IRecipeService, RecipeService>();
            services.AddTransient<IChefService, ChefService>();
            services.AddTransient<IIdentityService, IdentityService>();
        }

        private static void AddDataStores(IServiceCollection services, IConfiguration configuration)
        {
            AddCosmosDB(services, configuration);
            AddSqlDB(services, configuration);
        }

        private static void AddCosmosDB(IServiceCollection services, IConfiguration configuration)
        {
            var cosmosOptions = GetCosmosOptions(services, configuration);
            services.AddDbContext<CosmosContext>();
            services.AddHealthChecks().AddDbContextCheck<CosmosContext>("CosmosDB", customTestQuery: HealthCheck);
            CosmosContext.Create(cosmosOptions).GetAwaiter();

            Task<bool> HealthCheck(CosmosContext context, CancellationToken token)
            {
                return context.CanCosmosConnectAsync(cosmosOptions.DatabaseName, cosmosOptions.ContainerName, token);
            }
        }

        private static void AddSqlDB(IServiceCollection services, IConfiguration configuration)
        {
            var sqlConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddHealthChecks().AddDbContextCheck<SqlDbContext>("SqlDB");
            services.AddDbContext<SqlDbContext>(options => options.UseSqlServer(sqlConnectionString));
        }

        private static CosmosOptions GetCosmosOptions(IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(CosmosOptions.OptionsKey);
            services.Configure<CosmosOptions>(section);

            var options = section.Get<CosmosOptions>();
            return options;
        }
    }
}