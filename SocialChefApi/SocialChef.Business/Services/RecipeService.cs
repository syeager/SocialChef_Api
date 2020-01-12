using System.Threading.Tasks;
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
            documentContext.Recipes.Add(new Recipe());
            await Task.CompletedTask;
        }
    }
}