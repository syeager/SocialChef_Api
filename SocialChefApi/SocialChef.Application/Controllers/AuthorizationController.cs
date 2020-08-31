using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Server.AspNetCore;
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
        public async Task<IActionResult> Exchange()
        {
            var tokenRequest = new TokenRequest(HttpContext);
            var claimsPrincipal = await authorizationService.ExchangeToken(tokenRequest);
            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }
    }
}