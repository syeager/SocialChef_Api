using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace SocialChef.Domain.Identity
{
    public interface IAuthorizeRequest : IOidcRequest
    {
        Task<AuthenticateResult?> AuthenticateAsync(string scheme);
    }

    public class AuthorizeRequest : OidcRequest, IAuthorizeRequest
    {
        public AuthorizeRequest(HttpContext httpContext)
            : base(httpContext) {}

        public Task<AuthenticateResult?> AuthenticateAsync(string scheme)
        {
            return httpContext.AuthenticateAsync(scheme);
        }
    }
}