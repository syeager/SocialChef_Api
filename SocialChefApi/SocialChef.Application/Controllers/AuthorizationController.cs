using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialChef.Domain.Identity;

namespace SocialChef.Application.Controllers
{
    public class AuthorizationController : Controller
    {
        private readonly IAuthorizationService authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        [HttpGet("~/connect/authorize")]
        public Task<IActionResult> Authorize()
        {
            throw new NotImplementedException();
        }

        [HttpPost("~/connect/token")]
        public Task<IActionResult> Exchange()
        {
            throw new NotImplementedException();
        }

        [HttpPost("~/connect/logout"), ValidateAntiForgeryToken]
        public Task<IActionResult> LogoutPost()
        {
            throw new NotImplementedException();
        }
    }
}