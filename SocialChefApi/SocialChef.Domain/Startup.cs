using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LittleByte.Asp.Configuration;
using LittleByte.Asp.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Document;
using SocialChef.Domain.Identity;
using SocialChef.Domain.Recipes;
using SocialChef.Domain.Relational;

namespace SocialChef.Domain
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

            services.AddTransient<IIdentityService, IdentityService>();

            services.AddTransient<IChefCreator, ChefCreator>();
            services.AddTransient<IChefFinder, ChefFinder>();

            services.AddTransient<IRecipeCreator, RecipeCreator>();
            services.AddTransient<IRecipeFinder, RecipeFinder>();
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

        private static CosmosOptions GetCosmosOptions(IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(CosmosOptions.OptionsKey);
            services.Configure<CosmosOptions>(section);

            var options = section.Get<CosmosOptions>();
            return options;
        }

        private static void AddSqlDB(IServiceCollection services, IConfiguration configuration)
        {
            var sqlConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddHealthChecks().AddDbContextCheck<SqlDbContext>("SqlDB");
            services.AddDbContext<SqlDbContext>(options => options.UseSqlServer(sqlConnectionString));
        }
    }
}