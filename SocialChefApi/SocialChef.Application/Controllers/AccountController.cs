using System;
using System.Net;
using System.Threading.Tasks;
using LittleByte.Asp.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChef.Application.Dtos;
using SocialChef.Application.ViewModels.Account;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Identity;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace SocialChef.Application.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IChefCreator chefCreator;
        private readonly IAccountService accountService;

        public AccountController(IChefCreator chefCreator, IAccountService accountService)
        {
            this.chefCreator = chefCreator;
            this.accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ResponseType(HttpStatusCode.Created, typeof(ChefDto))]
        [ResponseType(HttpStatusCode.BadRequest)]
        public async Task<ApiResult<ChefDto>> Register(CreateChefDto dto)
        {
            var chef = await chefCreator.CreateAsync(dto.Name, dto.Email, dto.Password, dto.PasswordConfirm);
            return new CreatedResult<ChefDto>(chef);
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if(ModelState.IsValid)
            {
                var result = await accountService.LogInAsync(model.Email, model.Password, model.RememberMe);
                if(result.Succeeded)
                {
                    return RedirectPermanent(returnUrl);
                }

                if(result.RequiresTwoFactor)
                {
                    //return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    throw new NotImplementedException();
                }

                if(result.IsLockedOut)
                {
                    //return View("Lockout");
                    throw new NotImplementedException();
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            return View(model);
        }

        [HttpPost("logout"), ValidateAntiForgeryToken]
        public Task<IActionResult> LogoutPost()
        {
            throw new NotImplementedException();
        }
    }
}