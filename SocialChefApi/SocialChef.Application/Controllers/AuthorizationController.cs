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
        public async Task<IActionResult> Authorize()
        {
            var authorizeRequest = new AuthorizeRequest(HttpContext);
            var claimsPrincipal = await authorizationService.Authorize(authorizeRequest);

            if(claimsPrincipal == null)
            {
                return Challenge();
            }

            return SignIn(claimsPrincipal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
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