using System;
using System.Net;
using System.Threading.Tasks;
using LittleByte.Asp.Application;
using LittleByte.Asp.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChef.Domain.DTOs;
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
        public async Task<ActionResult<ApiResult<RecipeDto>>> Create(RecipeDto dto)
        {
            var domainModel = await recipeCreator.CreateAsync(dto);

            return CreatedAtAction("Get", new CreatedResult<RecipeDto>(domainModel));
        }

        [AllowAnonymous]
        [HttpGet("{recipeID}")]
        [ResponseType(HttpStatusCode.OK, typeof(RecipeDto))]
        [ResponseType(HttpStatusCode.NotFound)]
        public async Task<ApiResult<RecipeDto>> Get(Guid recipeID)
        {
            var id = new Recipe.Guid(recipeID);
            var domainModel = await recipeFinder.FindByIdAsync(id);

            return new OkResult<RecipeDto>(domainModel);
        }

        [AllowAnonymous]
        [HttpGet("")]
        [ResponseType(HttpStatusCode.OK, typeof(PageResponse<RecipeDto>))]
        [ResponseType(HttpStatusCode.NotFound)]
        public async Task<ApiResult<PageResponse<RecipeDto>>> Get([FromQuery] PageRequest request)
        {
            var dto = await recipeFinder.GetLatest(request);

            var response = dto.CastResults(r => (RecipeDto)r);
            return new OkResult<PageResponse<RecipeDto>>(response);
        }

        [AllowAnonymous]
        [HttpGet("chef/{chefID}")]
        [ResponseType(HttpStatusCode.OK, typeof(PageResponse<RecipeDto>))]
        [ResponseType(HttpStatusCode.NotFound)]
        public async Task<ApiResult<PageResponse<RecipeDto>>> GetByChef(Guid chefID, [FromQuery] PageRequest request)
        {
            var chefId = new Chef.Guid(chefID);
            var dto = await recipeFinder.FindByChefAsync(chefId, request);

            var response = dto.CastResults(r => (RecipeDto)r);
            return new OkResult<PageResponse<RecipeDto>>(response);
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