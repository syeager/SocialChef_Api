using System.Threading.Tasks;
using SocialChef.Business.DTOs;
using SocialChef.Persistence;

namespace SocialChef.Business.Services
{
    public interface IRecipeService
    {
        Task<RecipeDto> CreateAsync(string name);
    }

    internal class RecipeService : IRecipeService
    {
        private readonly CosmosContext documentContext;

        public RecipeService(CosmosContext documentContext)
        {
            this.documentContext = documentContext;
        }

        public async Task<RecipeDto> CreateAsync(string name)
        {
            documentContext.Recipes.Add(new Recipe(name));
            await documentContext.SaveChangesAsync();

            // how to convert dao to dto
            // 
            return new RecipeDto("");
        }
    }
}