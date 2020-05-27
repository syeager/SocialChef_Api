using System;
using System.Net;
using System.Threading.Tasks;
using LittleByte.Asp.Application;
using LittleByte.Asp.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChef.Application.Dtos.Recipes;
using SocialChef.Domain.Recipes;
using Controller = LittleByte.Asp.Application.Controller;

namespace SocialChef.Application.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IRecipeCreator recipeCreator;
        private readonly IRecipeFinder recipeFinder;

        public RecipeController(IRecipeCreator recipeCreator, IRecipeFinder recipeFinder)
        {
            this.recipeCreator = recipeCreator;
            this.recipeFinder = recipeFinder;
        }

        [HttpPost]
        [ResponseType(HttpStatusCode.Created, typeof(RecipeDto))]
        public async Task<ApiResult<RecipeDto>> Create(RecipeDto dto)
        {
            var domainModel = await recipeCreator.CreateAsync(dto);

            return new CreatedResult<RecipeDto>(domainModel);
        }

        [AllowAnonymous]
        [HttpGet("{recipeID}")]
        [ResponseType(HttpStatusCode.OK, typeof(RecipeDto))]
        [ResponseType(HttpStatusCode.NotFound)]
        public async Task<ApiResult<RecipeDto>> Get(Guid recipeID)
        {
            var domainModel = await recipeFinder.FindByIdAsync(recipeID);

            return new OkResult<RecipeDto>(domainModel);
        }

        [AllowAnonymous]
        [HttpGet("")]
        [ResponseType(HttpStatusCode.OK, typeof(PageResponse<RecipeSummaryDto>))]
        [ResponseType(HttpStatusCode.NotFound)]
        public async Task<ApiResult<PageResponse<RecipeSummaryDto>>> Get([FromQuery] PageRequest request)
        {
            var dto = await recipeFinder.GetLatest(request);

            var response = dto.CastResults(r => (RecipeSummaryDto)r);
            return new OkResult<PageResponse<RecipeSummaryDto>>(response);
        }

        [AllowAnonymous]
        [HttpGet("chef/{chefID}")]
        [ResponseType(HttpStatusCode.OK, typeof(PageResponse<RecipeSummaryDto>))]
        [ResponseType(HttpStatusCode.NotFound)]
        public async Task<ApiResult<PageResponse<RecipeSummaryDto>>> GetByChef(Guid chefID, [FromQuery] PageRequest request)
        {
            var dto = await recipeFinder.FindByChefAsync(chefID, request);

            var response = dto.CastResults(r => (RecipeSummaryDto)r);
            return new OkResult<PageResponse<RecipeSummaryDto>>(response);
        }

        //[HttpDelete("{recipeID}")]
        //[ResponseType(HttpStatusCode.NoContent)]
        //[ResponseType(HttpStatusCode.NotFound)]
        //public async Task<ApiResult> Delete(Guid recipeID)
        //{
        //    await recipeService.DeleteAsync(recipeID);
        //    return new DeletedResult<RecipeDto>(recipeID);
        //}
    }
}