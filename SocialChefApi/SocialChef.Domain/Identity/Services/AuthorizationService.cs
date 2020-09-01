using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using LittleByte.Asp.Exceptions;
using LittleByte.Asp.Extensions;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Core;
using OpenIddict.EntityFrameworkCore.Models;
using OpenIddict.Server.AspNetCore;
using SocialChef.Domain.Relational;
using static OpenIddict.Abstractions.OpenIddictConstants.Errors;
using static OpenIddict.Abstractions.OpenIddictConstants.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static Microsoft.AspNetCore.Identity.IdentityConstants;

namespace SocialChef.Domain.Identity
{
    public interface IAuthorizationService
    {
        Task<ClaimsPrincipal?> Authorize(IAuthorizeRequest authorizeRequest);
        Task<ClaimsPrincipal> ExchangeToken(ITokenRequest tokenRequest);
    }

    internal class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<UserDao> userManager;
        private readonly SignInManager<UserDao> signInManager;
        private readonly OpenIddictScopeManager<OpenIddictEntityFrameworkCoreScope> scopeManager;

        public AuthorizationService(UserManager<UserDao> userManager, SignInManager<UserDao> signInManager, OpenIddictScopeManager<OpenIddictEntityFrameworkCoreScope> scopeManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.scopeManager = scopeManager;
        }

        public async Task<ClaimsPrincipal?> Authorize(IAuthorizeRequest authorizeRequest)
        {
            var request = GetRequest(authorizeRequest);

            var result = await authorizeRequest.AuthenticateAsync(ApplicationScheme);
            if(result == null || !result.Succeeded)
            {
                ThrowIfNoPrompt();
                return null;
            }

            if(request.HasPrompt(Prompts.Login))
            {
                var prompt = string.Join(" ", request.GetPrompts().Remove(Prompts.Login));
                throw new NotImplementedException();
//#error continue. Need to find solution for needing httpcontext
            }

            // TODO: Create DateService.
            if(request.MaxAge != null && result.Properties?.IssuedUtc != null && DateTimeOffset.UtcNow - result.Properties.IssuedUtc > TimeSpan.FromSeconds(request.MaxAge.Value))
            {
                ThrowIfNoPrompt();
                return null;
            }

            var user = await userManager.GetUserAsync(result.Principal);
            if(user == null)
            {
                throw new NotFoundException(typeof(User), Guid.Empty);
            }

            var principal = await signInManager.CreateUserPrincipalAsync(user);
            var scopes = request.GetScopes();
            var resources = await scopeManager.ListResourcesAsync(scopes).ToListAsync();
            principal.SetScopes(scopes);
            principal.SetResources(resources);

            foreach(var claim in principal.Claims)
            {
                claim.SetDestinations(GetDestinations(claim, principal));
            }

            return principal;

            void ThrowIfNoPrompt()
            {
                if(request.HasPrompt(Prompts.None))
                {
                    throw new ForbiddenException(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, LoginRequired, "The user is not logged in.");
                }
            }
        }

        public async Task<ClaimsPrincipal> ExchangeToken(ITokenRequest tokenRequest)
        {
            var request = GetRequest(tokenRequest);

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

        private static OpenIddictRequest GetRequest(IOidcRequest tokenRequest)
        {
            var request = tokenRequest.GetOpenIddictServerRequest();
            if(request == null)
            {
                throw new BadRequestException("The OpenID Connect request cannot be retrieved.");
            }

            return request;
        }

        private static IEnumerable<string> GetDestinations(Claim claim, ClaimsPrincipal principal)
        {
            // Note: by default, claims are NOT automatically included in the access and identity tokens.
            // To allow OpenIddict to serialize them, you must attach them a destination, that specifies
            // whether they should be included in access tokens, in identity tokens or in both.

            switch(claim.Type)
            {
                case Name:
                    yield return Destinations.AccessToken;

                    if(principal.HasScope(Permissions.Scopes.Profile))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Email:
                    yield return Destinations.AccessToken;

                    if(principal.HasScope(Permissions.Scopes.Email))
                        yield return Destinations.IdentityToken;

                    yield break;

                case Role:
                    yield return Destinations.AccessToken;

                    if(principal.HasScope(Permissions.Scopes.Roles))
                        yield return Destinations.IdentityToken;

                    yield break;

                // Never include the security stamp in the access and identity tokens, as it's a secret value.
                case "AspNet.Identity.SecurityStamp": yield break;

                default:
                    yield return Destinations.AccessToken;
                    yield break;
            }
        }
    }
}