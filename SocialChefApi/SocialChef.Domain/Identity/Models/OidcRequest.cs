using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using OpenIddict.Abstractions;

namespace SocialChef.Domain.Identity
{
    public interface IOidcRequest
    {
        OpenIddictRequest? GetOpenIddictServerRequest();
    }

    public abstract class OidcRequest : IOidcRequest
    {
        protected readonly HttpContext httpContext;

        protected OidcRequest(HttpContext httpContext)
        {
            this.httpContext = httpContext;
        }

        public OpenIddictRequest? GetOpenIddictServerRequest() => httpContext.GetOpenIddictServerRequest();
    }
}