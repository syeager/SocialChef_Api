using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace SocialChef.Domain.Identity
{
    public interface ITokenRequest : IOidcRequest
    {
        Task<AuthenticateResult> AuthenticateAsync(string scheme);
    }

    public class TokenRequest : OidcRequest, ITokenRequest
    {
        public TokenRequest(HttpContext httpContext)
            : base(httpContext) {}

        public Task<AuthenticateResult> AuthenticateAsync(string scheme) => httpContext.AuthenticateAsync(scheme);
    }
}