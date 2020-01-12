using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialChef.Business.Services;

namespace SocialChef.Business
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            AddServices(serviceCollection);
            Persistence.Startup.ConfigureServices(serviceCollection, configuration);
        }

        private static void AddServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IRecipeService, RecipeService>();
        }
    }
}