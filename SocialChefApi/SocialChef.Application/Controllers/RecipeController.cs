﻿using System.Threading.Tasks;
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
        public async Task<ActionResult<RecipeDto>> Create(CreateRecipeRequest request)
        {
            var dto = await recipeService.CreateAsync(request);

            return CreatedAtAction(nameof(Get), new {recipeID = dto.ID}, dto);
        }

        [HttpGet]
        public async Task<ActionResult<RecipeDto>> Get(string recipeID)
        {
            var dto = await recipeService.GetAsync(recipeID);

            return Ok(dto);
        }
    }
}