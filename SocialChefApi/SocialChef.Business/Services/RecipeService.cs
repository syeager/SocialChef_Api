using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Linq;
using SocialChef.Persistence;

namespace SocialChef.Business.Services
{
    public interface IRecipeService
    {
        Task CreateAsync();
    }

    internal class RecipeService : IRecipeService
    {
        private readonly CosmosContext documentContext;

        public RecipeService(CosmosContext documentContext)
        {
            this.documentContext = documentContext;
        }

        public async Task CreateAsync()
        {
            var count = await documentContext.Recipes.CountAsync();
            documentContext.Recipes.Add(new Recipe($"New Recipe - {count + 1}"));
            await documentContext.SaveChangesAsync();
        }
    }
}