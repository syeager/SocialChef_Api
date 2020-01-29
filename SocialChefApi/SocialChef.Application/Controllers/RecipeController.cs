﻿using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using LittleByte.Asp.Application;
using Microsoft.AspNetCore.Mvc;
using SocialChef.Business.DTOs;
using SocialChef.Business.Requests;
using SocialChef.Business.Services;

namespace SocialChef.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        [HttpPost]
        [ResponseType(HttpStatusCode.Created, typeof(RecipeDto))]
        public async Task<ApiResult<RecipeDto>> Create(CreateRecipeRequest request)
        {
            var dto = await recipeService.CreateAsync(request);
            return new CreatedResult<RecipeDto>(dto);
        }

        [HttpGet("{recipeID}")]
        [ResponseType(HttpStatusCode.OK, typeof(RecipeDto))]
        [ResponseType(HttpStatusCode.NotFound)]
        public async Task<ApiResult<RecipeDto>> Get([Required] string recipeID)
        {
            var dto = await recipeService.GetAsync(recipeID);
            return new OkResult<RecipeDto>(dto);
        }

        [HttpDelete("{recipeID}")]
        [ResponseType(HttpStatusCode.NoContent)]
        [ResponseType(HttpStatusCode.NotFound)]
        public async Task<ApiResult> Delete([Required] string recipeID)
        {
            await recipeService.DeleteAsync(recipeID);
            return new DeletedResult<RecipeDto>(recipeID);
        }
    }
}