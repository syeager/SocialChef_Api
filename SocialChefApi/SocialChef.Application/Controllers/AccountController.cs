using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialChef.Application.ViewModels.Account;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Identity;

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

        //[AllowAnonymous]
        //[HttpPost("register")]
        //[ResponseType(HttpStatusCode.Created, typeof(ChefDto))]
        //[ResponseType(HttpStatusCode.BadRequest)]
        //public async Task<ApiResult<ChefDto>> Register(CreateChefDto dto)
        //{
        //    var chef = await chefCreator.CreateAsync(dto.Name, dto.Email, dto.Password, dto.PasswordConfirm);
        //    return new CreatedResult<ChefDto>(chef);
        //}

        [HttpGet("register")]
        [AllowAnonymous]
        public IActionResult Register(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if(ModelState.IsValid)
            {
                var result = await accountService.RegisterAsync(model.Email, model.Password, model.ConfirmPassword);

                if(result.Succeeded)
                {
                    await accountService.LogInAsync(model.Email, model.Password, false);
                    return Redirect(returnUrl);
                }
            }

            return View(model);
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