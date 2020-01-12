using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SocialChef.Persistence
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CosmosOptions>(configuration.GetSection(CosmosOptions.OptionsKey));

            services.AddDbContext<CosmosContext>();
        }
    }
}