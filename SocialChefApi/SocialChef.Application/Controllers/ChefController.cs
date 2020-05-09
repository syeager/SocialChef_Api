using System;
using System.Net;
using System.Threading.Tasks;
using LittleByte.Asp.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Recipes;
using Controller = LittleByte.Asp.Application.Controller;

namespace SocialChef.Application.Controllers
{
    public class ChefController : Controller
    {
        private readonly IChefCreator chefCreator;
        private readonly IChefFinder chefFinder;

        public ChefController(IChefCreator chefCreator, IChefFinder chefFinder)
        {
            this.chefCreator = chefCreator;
            this.chefFinder = chefFinder;
        }

        [AllowAnonymous]
        [HttpPost]
        [ResponseType(HttpStatusCode.Created, typeof(ChefDto))]
        public async Task<ActionResult<ApiResult<ChefDto>>> Create(CreateChefRequest request)
        {
            var chef = await chefCreator.CreateAsync(request);

            return CreatedAtAction("Get", new {chefID= chef.ID}, new CreatedResult<ChefDto>(chef));
        }

        [HttpGet("user/{userID}")]
        [ResponseType(HttpStatusCode.OK, typeof(ChefDto))]
        public async Task<ApiResult<ChefDto>> GetByUser(Guid userID)
        {
            var userDomainId = new User.Guid(userID);

            var dto = await chefFinder.FindByUserAsync(userDomainId);
            return new OkResult<ChefDto>(dto);
        }

        [AllowAnonymous]
        [HttpGet("{chefID}")]
        [ResponseType(HttpStatusCode.OK, typeof(ChefDto))]
        public async Task<ApiResult<ChefDto>> Get(Guid chefID)
        {
            var chefDomainId = new Chef.Guid(chefID);

            var dto = await chefFinder.FindByIdAsync(chefDomainId);
            return new OkResult<ChefDto>(dto);
        }
    }
}