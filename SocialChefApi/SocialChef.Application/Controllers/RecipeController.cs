using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialChef.Business.DTOs;
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
        public async Task<ActionResult<RecipeDto>> Create()
        {
            var dto = await recipeService.CreateAsync("");
            return Created("", dto);
        }
    }
}