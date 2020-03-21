using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleByte.Asp.Business;
using LittleByte.Asp.Database;
using LittleByte.Asp.Exceptions;
using Microsoft.EntityFrameworkCore;
using SocialChef.Business.Document.Contexts;
using SocialChef.Business.Document.Models;
using SocialChef.Business.DTOs;
using SocialChef.Business.Relational.Contexts;
using SocialChef.Business.Relational.Models;
using SocialChef.Business.Requests;

namespace SocialChef.Business.Services
{
    public interface IRecipeService
    {
        Task<RecipeDto> CreateAsync(CreateRecipeRequest request);
        Task<RecipeDto> GetAsync(Guid entityID);
        Task<PageResponse<RecipeDto>> GetAsync(PageRequest request);
        // TODO: Return PageResposne<ReciptDto>.
        Task<IReadOnlyList<RecipeDto>> GetForChefAsync(GetChefsRecipesRequests request);
        Task DeleteAsync(Guid entityID);
    }

    internal class RecipeService : IRecipeService
    {
        private readonly CosmosContext documentContext;
        private readonly SqlDbContext sqlContext;
        private readonly IChefService chefService;

        public RecipeService(CosmosContext documentContext, SqlDbContext sqlContext, IChefService chefService)
        {
            this.documentContext = documentContext;
            this.sqlContext = sqlContext;
            this.chefService = chefService;
        }

        public async Task<RecipeDto> CreateAsync(CreateRecipeRequest request)
        {
            if(request.Steps == null || request.Steps.Count == 0)
            {
                throw new BadRequestException("Can't create a recipe without steps.");
            }

            if(await sqlContext.Chefs.AllAsync(c => c.ID != request.ChefID))
            {
                throw new NotFoundException(typeof(ChefDto), request.ChefID);
            }

            var entity = new Recipe(request.ChefID, request.Name, request.Steps);

            await documentContext.AddAndSaveAsync(entity);

            var chefRecipe = new ChefRecipe(entity.ChefID, entity.ID);
            await sqlContext.AddAndSaveAsync(chefRecipe);

            return ToDto(entity, new ChefDto());
        }

        public async Task<RecipeDto> GetAsync(Guid entityID)
        {
            var entity = await FindEntity(entityID);
            var chef = await chefService.GetChefAsync(entity.ChefID);

            return ToDto(entity, chef);
        }

        public async Task<PageResponse<RecipeDto>> GetAsync(PageRequest request)
        {
            var pageCount = await documentContext.Recipes.PageCountAsync(request.PageSize);

            var entities = await documentContext.Recipes
                .Page(request.PageSize, request.Page)
                .ToListAsync();

            var chefIDs = entities
                .Select(r => r.ChefID)
                .ToHashSet();
            var chefEntities = await sqlContext.Chefs
                .Where(c => chefIDs.Contains(c.ID))
                .ToDictionaryAsync(c => c.ID, c => c);

            var recipes = entities.Select(ToDto).ToArray();
            var dto = new PageResponse<RecipeDto>(recipes, request.Page, pageCount);
            return dto;

            RecipeDto ToDto(Recipe r) => this.ToDto(r, chefService.ToDto(chefEntities[r.ChefID]));
        }

        public async Task<IReadOnlyList<RecipeDto>> GetForChefAsync(GetChefsRecipesRequests request)
        {
            var chefExists = await sqlContext.Chefs.AnyAsync(c => c.ID == request.ChefID);
            if(!chefExists)
            {
                throw new NotFoundException(typeof(ChefDto), request.ChefID);
            }

            var entities = await documentContext.Recipes
                .Where(r => r.ChefID == request.ChefID)
                .Page(request.PageSize, request.Page)
                .ToListAsync();
            return entities.Select(r => ToDto(r, new ChefDto())).ToArray();
        }

        public async Task DeleteAsync(Guid entityID)
        {
            var entity = await FindEntity(entityID);
            await documentContext.RemoveAndSaveAsync(entity);
        }

        private async Task<Recipe> FindEntity(Guid entityID)
        {
            var entity = await documentContext.Recipes.FindAsync(entityID);

            if(entity == null)
            {
                throw new NotFoundException(typeof(RecipeDto), entityID);
            }

            return entity;
        }

        private RecipeDto ToDto(Recipe entity, ChefDto chef)
        {
            return new RecipeDto(entity.ID, entity.Name, entity.Steps.Select(s => s.Instruction).ToArray(), chef);
        }
    }
}