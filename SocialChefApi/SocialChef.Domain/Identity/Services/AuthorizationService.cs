using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using LittleByte.Asp.Exceptions;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using SocialChef.Domain.Relational;
using static OpenIddict.Abstractions.OpenIddictConstants.Errors;
using static OpenIddict.Abstractions.OpenIddictConstants.Claims;

namespace SocialChef.Domain.Identity
{
    public interface IAuthorizationService
    {
        Task<ClaimsPrincipal> ExchangeToken(ITokenRequest tokenRequest);
    }

    internal class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<UserDao> userManager;
        private readonly SignInManager<UserDao> signInManager;

        public AuthorizationService(UserManager<UserDao> userManager, SignInManager<UserDao> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<ClaimsPrincipal> ExchangeToken(ITokenRequest tokenRequest)
        {
            var request = tokenRequest.GetOpenIddictServerRequest();
            if(request == null)
            {
                throw new BadRequestException("The OpenID Connect request cannot be retrieved.");
            }

            if(!(request.IsClientCredentialsGrantType() || request.IsRefreshTokenGrantType()))
            {
                throw new BadRequestException($"The '{request.GrantType}' grant type is not supported.");
            }

            var principal = (await tokenRequest.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme)).Principal;
            var user = await userManager.GetUserAsync(principal);
            if(user == null)
            {
                throw new ForbiddenException(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, InvalidGrant, "The token is no longer valid.");
            }

            var canSignIn = await signInManager.CanSignInAsync(user);
            if(!canSignIn)
            {
                throw new ForbiddenException(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, InvalidGrant, "The user is no longer allowed to sign in.");
            }

            foreach(var claim in principal.Claims)
            {
                claim.SetDestinations(GetDestinations(claim, principal));
            }

            return principal;
        }

        private static IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            switch(claim.Type)
            {
                case Name:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if(principal.HasScope(OpenIddictConstants.Permissions.Scopes.Profile))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;

                case Email:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if(principal.HasScope(OpenIddictConstants.Permissions.Scopes.Email))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;

                case Role:
                    yield return OpenIddictConstants.Destinations.AccessToken;

                    if(principal.HasScope(OpenIddictConstants.Permissions.Scopes.Roles))
                        yield return OpenIddictConstants.Destinations.IdentityToken;

                    yield break;

                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                case "AspNet.Identity.SecurityStamp": yield break;

                default:
                    yield return OpenIddictConstants.Destinations.AccessToken;
                    yield break;
            }
        }
    }
}