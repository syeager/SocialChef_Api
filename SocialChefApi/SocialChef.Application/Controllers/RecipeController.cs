using System.ComponentModel.DataAnnotations;
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
        [ResponseType(typeof(RecipeDto), HttpStatusCode.Created)]
        public async Task<ApiResult<RecipeDto>> Create(CreateRecipeRequest request)
        {
            var dto = await recipeService.CreateAsync(request);
            return new ApiResult<RecipeDto>(HttpStatusCode.Created, dto);
        }

        [HttpGet("{recipeID}")]
        [ResponseType(typeof(RecipeDto), HttpStatusCode.OK)]
        [ResponseType(typeof(RecipeDto), HttpStatusCode.NotFound)]
        public async Task<ApiResult<RecipeDto>> Get([Required] string recipeID)
        {
            var dto = await recipeService.GetAsync(recipeID);
            return new ApiResult<RecipeDto>(HttpStatusCode.OK, dto);
        }
    }
}