using System;
using System.Net;
using System.Threading.Tasks;
using LittleByte.Asp.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChef.Application.Dtos;
using SocialChef.Domain.Chefs;
using Controller = LittleByte.Asp.Application.Controller;

namespace SocialChef.Application.Controllers
{
    public class AccountController : Controller
    {
        private readonly IChefCreator chefCreator;

        public AccountController(IChefCreator chefCreator)
        {
            this.chefCreator = chefCreator;
        }

        [AllowAnonymous]
        [HttpPost]
        [ResponseType(HttpStatusCode.Created, typeof(ChefDto))]
        [ResponseType(HttpStatusCode.BadRequest)]
        public async Task<ApiResult<ChefDto>> Register(CreateChefDto dto)
        {
            var chef = await chefCreator.CreateAsync(dto.Name, dto.Email, dto.Password, dto.PasswordConfirm);
            return new CreatedResult<ChefDto>(chef);
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public async Task<IActionResult> Login(string returnUrl)
        {
            await Task.CompletedTask;
            return Ok($"Hello! You wanted to go to: '{returnUrl}'");
        }

        [HttpPost("logout"), ValidateAntiForgeryToken]
        public Task<IActionResult> LogoutPost()
        {
            throw new NotImplementedException();
        }
    }
}