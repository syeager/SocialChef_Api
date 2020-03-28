using System;
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
        Task<PageResponse<RecipeDto>> GetForChefAsync(Guid chefID, PageRequest request);
        Task DeleteAsync(Guid entityID);
    }

    internal sealed class RecipeService : EntityService<Recipe, CosmosContext>, IRecipeService
    {
        private readonly SqlDbContext sqlContext;
        private readonly IChefService chefService;

        public RecipeService(CosmosContext documentContext, SqlDbContext sqlContext, IChefService chefService)
            : base(documentContext)
        {
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

            await DBContext.AddAndSaveAsync(entity);

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
            var entities = await DBContext.Recipes.GetPagedAsync(request.PageSize, request.Page);

            var chefIDs = entities.Results
                .Select(r => r.ChefID)
                .ToHashSet();
            var chefEntities = await sqlContext.Chefs
                .Where(c => chefIDs.Contains(c.ID))
                .ToDictionaryAsync(c => c.ID, c => c);

            var recipes = entities.Results.Select(ToDto).ToArray();
            var dto = new PageResponse<RecipeDto>(entities.PageSize, entities.Page, entities.TotalPages, entities.TotalResults, recipes);
            return dto;

            RecipeDto ToDto(Recipe r) => this.ToDto(r, chefService.ToDto(chefEntities[r.ChefID]));
        }

        public async Task<PageResponse<RecipeDto>> GetForChefAsync(Guid chefID, PageRequest request)
        {
            var chefExists = await sqlContext.Chefs.AnyAsync(c => c.ID == chefID);
            if(!chefExists)
            {
                throw new NotFoundException(typeof(ChefDto), chefID);
            }

            var entities = await DBContext.Recipes
                .Where(r => r.ChefID == chefID)
                .GetPagedAsync(request.PageSize, request.Page);
            var recipes = entities.Results.Select(r => ToDto(r, new ChefDto())).ToArray();

            var dto = new PageResponse<RecipeDto>(entities.PageSize, entities.Page, entities.TotalPages, entities.TotalResults, recipes);
            return dto;
        }

        private RecipeDto ToDto(Recipe entity, ChefDto chef)
        {
            return new RecipeDto(entity.ID, entity.Name, entity.Steps.Select(s => s.Instruction).ToArray(), chef);
        }
    }
}