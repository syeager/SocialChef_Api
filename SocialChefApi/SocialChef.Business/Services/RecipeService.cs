using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleByte.Asp.Business;
using LittleByte.Asp.Database;
using LittleByte.Asp.Exceptions;
using Microsoft.EntityFrameworkCore;
using SocialChef.Business.DTOs;
using SocialChef.Business.Requests;
using SocialChef.Persistence;

namespace SocialChef.Business.Services
{
    public interface IRecipeService
    {
        Task<RecipeDto> CreateAsync(CreateRecipeRequest request);
        Task<RecipeDto> GetAsync(string entityID);
        Task<IReadOnlyList<RecipeDto>> GetAsync(PageRequest request);
        Task DeleteAsync(string entityID);
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
            if(request.Steps == null || request.Steps.Count == 0)
            {
                throw new BadRequestException("Can't create a recipe without steps.");
            }

            var entity = new Recipe(request.Name, request.Steps);

            documentContext.Recipes.Add(entity);
            await documentContext.SaveChangesAsync();

            return ToDto(entity);
        }

        public async Task<RecipeDto> GetAsync(string entityID)
        {
            var entity = await FindEntity(entityID);
            return ToDto(entity);
        }

        public async Task<IReadOnlyList<RecipeDto>> GetAsync(PageRequest request)
        {
            var entities = await documentContext.Recipes
                .Page(request.PageSize, request.Page)
                .ToListAsync();
            return entities.Select(ToDto).ToArray();
        }

        public async Task DeleteAsync(string entityID)
        {
            var entity = await FindEntity(entityID);
            documentContext.Recipes.Remove(entity);
            await documentContext.SaveChangesAsync();
        }

        private async Task<Recipe> FindEntity(string entityID)
        {
            var entity = await documentContext.Recipes.FindAsync(entityID);

            if(entity == null)
            {
                throw new NotFoundException(typeof(RecipeDto), entityID);
            }

            return entity;
        }

        private static RecipeDto ToDto(Recipe entity)
        {
            return new RecipeDto(entity.ID, entity.Name);
        }
    }
}