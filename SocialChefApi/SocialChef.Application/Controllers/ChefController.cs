using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LittleByte.Asp.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChef.Application.Dtos.Chefs;
using SocialChef.Domain.Chefs;
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
        [ResponseType(HttpStatusCode.BadRequest)]
        public async Task<ApiResult<ChefDto>> Create(CreateChefDto dto)
        {
            var chef = await chefCreator.CreateAsync(dto.Name, dto.Email, dto.Password, dto.PasswordConfirm);
            return new CreatedResult<ChefDto>(chef);
        }

        [HttpGet("user/{userID}")]
        [ResponseType(HttpStatusCode.OK, typeof(ChefDto))]
        [ResponseType(HttpStatusCode.NotFound)]
        public async Task<ApiResult<ChefDto>> GetByUser(Guid userID)
        {
            var dto = await chefFinder.FindByUserAsync(userID);
            return new OkResult<ChefDto>(dto);
        }

        [AllowAnonymous]
        [HttpGet("{chefID}")]
        [ResponseType(HttpStatusCode.OK, typeof(ChefDto))]
        [ResponseType(HttpStatusCode.NotFound)]
        public async Task<ApiResult<ChefDto>> Get(Guid chefID)
        {
            var dto = await chefFinder.FindByIdAsync(chefID);
            return new OkResult<ChefDto>(dto);
        }

        // TODO: Handle too many chefs requested. Middleware?
        [AllowAnonymous]
        [HttpGet("batch")]
        [ResponseType(HttpStatusCode.OK, typeof(ChefDto[]))]
        public async Task<ApiResult<IReadOnlyList<ChefDto>>> GetByBatch(List<Guid> chefIds)
        {
            var chefs = await chefFinder.FindByIdsAsync(chefIds);
            var dto = chefs.Select(c => (ChefDto)c).ToList();
            return new OkResult<IReadOnlyList<ChefDto>>(dto);
        }
    }
}