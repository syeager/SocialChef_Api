using System;
using System.Net;
using System.Threading.Tasks;
using LittleByte.Asp.Application;
using LittleByte.Asp.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChef.Business.DTOs;
using SocialChef.Business.Requests;
using SocialChef.Business.Services;
using Controller = LittleByte.Asp.Application.Controller;

namespace SocialChef.Application.Controllers
{
    public class RecipeController : Controller
    {
        private readonly IRecipeService recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        [HttpPost]
        [ResponseType(HttpStatusCode.Created, typeof(RecipeDto))]
        public async Task<ActionResult<ApiResult<RecipeDto>>> Create(CreateRecipeRequest request)
        {
            var dto = await recipeService.CreateAsync(request);
            return CreatedAtAction("Get", new CreatedResult<RecipeDto>(dto));
        }

        [AllowAnonymous]
        [HttpGet("{recipeID}")]
        [ResponseType(HttpStatusCode.OK, typeof(RecipeDto))]
        [ResponseType(HttpStatusCode.NotFound)]
        public async Task<ApiResult<RecipeDto>> Get(Guid recipeID)
        {
            var dto = await recipeService.GetAsync(recipeID);
            return new OkResult<RecipeDto>(dto);
        }
        
        [AllowAnonymous]
        [HttpGet("")]
        [ResponseType(HttpStatusCode.OK, typeof(PageResponse<RecipeDto>))]
        [ResponseType(HttpStatusCode.NotFound)]
        public async Task<ApiResult<PageResponse<RecipeDto>>> Get([FromQuery] PageRequest request)
        {
            var dto = await recipeService.GetAsync(request);
            return new OkResult<PageResponse<RecipeDto>>(dto);
        }
        
        [AllowAnonymous]
        [HttpGet("chef/{chefID}")]
        [ResponseType(HttpStatusCode.OK, typeof(PageResponse<RecipeDto>))]
        [ResponseType(HttpStatusCode.NotFound)]
        public async Task<ApiResult<PageResponse<RecipeDto>>> GetByChef(Guid chefID, [FromQuery] PageRequest request)
        {
            var dto = await recipeService.GetForChefAsync(chefID, request);
            return new OkResult<PageResponse<RecipeDto>>(dto);
        }

        [HttpDelete("{recipeID}")]
        [ResponseType(HttpStatusCode.NoContent)]
        [ResponseType(HttpStatusCode.NotFound)]
        public async Task<ApiResult> Delete(Guid recipeID)
        {
            await recipeService.DeleteAsync(recipeID);
            return new DeletedResult<RecipeDto>(recipeID);
        }
    }
}