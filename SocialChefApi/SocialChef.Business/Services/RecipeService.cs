using System.Threading.Tasks;
using LittleByte.Asp.Exceptions;
using SocialChef.Business.DTOs;
using SocialChef.Business.Requests;
using SocialChef.Persistence;

namespace SocialChef.Business.Services
{
    public interface IRecipeService
    {
        Task<RecipeDto> CreateAsync(CreateRecipeRequest request);
        Task<RecipeDto> GetAsync(string entityID);
    }

    internal class RecipeService : IRecipeService
    {
        private readonly CosmosContext documentContext;

        public RecipeService(CosmosContext documentContext)
        {
            this.documentContext = documentContext;
        }

        public async Task<RecipeDto> CreateAsync(CreateRecipeRequest request)
        {
            var entity = new Recipe(request.Name);
            documentContext.Recipes.Add(entity);
            await documentContext.SaveChangesAsync();

            return ToDto(entity);
        }

        public async Task<RecipeDto> GetAsync(string entityID)
        {
            var entity = await documentContext.Recipes.FindAsync(entityID);

            if(entity == null)
            {
                throw new NotFoundException(typeof(RecipeDto), entityID);
            }

            return ToDto(entity);
        }

        private static RecipeDto ToDto(Recipe entity)
        {
            return new RecipeDto(entity.ID, entity.Name);
        }
    }
}