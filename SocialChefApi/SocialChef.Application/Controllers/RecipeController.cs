using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Create()
        {
            await recipeService.CreateAsync();
            return Ok();
        }
    }
}