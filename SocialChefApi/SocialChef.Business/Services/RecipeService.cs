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
            var entity = new Recipe(name);
            documentContext.Recipes.Add(entity);
            await documentContext.SaveChangesAsync();

            return ToDto(entity);
        }

        private static RecipeDto ToDto(Recipe entity)
        {
            return new RecipeDto(entity.ID, entity.Name);
        }
    }
}