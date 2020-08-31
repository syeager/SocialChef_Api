using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using OpenIddict.Abstractions;

namespace SocialChef.Domain.Identity
{
    public interface ITokenRequest
    {
        OpenIddictRequest? GetOpenIddictServerRequest();
        Task<AuthenticateResult> AuthenticateAsync(string scheme);
    }

    public class TokenRequest : ITokenRequest
    {
        private readonly HttpContext httpContext;

        public TokenRequest(HttpContext httpContext)
        {
            this.httpContext = httpContext;
        }

        public OpenIddictRequest? GetOpenIddictServerRequest() => httpContext.GetOpenIddictServerRequest();
        public Task<AuthenticateResult> AuthenticateAsync(string scheme) => httpContext.AuthenticateAsync(scheme);
    }
}