using System;
using System.Net;
using System.Threading.Tasks;
using LittleByte.Asp.Application;
using LittleByte.Asp.Exceptions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
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
        public async Task<ActionResult<RecipeDto>> Create(CreateRecipeRequest request)
        {
            var dto = await recipeService.CreateAsync(request);

            return CreatedAtAction(nameof(Get), new {recipeID = dto.ID}, dto);
        }

        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ApiResponse<RecipeDto>), Description = "hey")]
        public async Task<ApiResponse<RecipeDto>> Get(string recipeID)
        {
            try
            {
                RecipeDto dto = await recipeService.GetAsync(recipeID);
                var response = ApiResponse<RecipeDto>.Success(HttpStatusCode.OK, dto);
                return response;
            }
            catch(HttpException exception)
            {
                var response = HandleHttpException<RecipeDto>(exception);
                return response;
            }
        }
    }
}