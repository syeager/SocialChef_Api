using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SocialChef.Identity.Models;

namespace SocialChef.Identity.Quickstart.Account
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class ExternalController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IIdentityServerInteractionService interaction;
        private readonly IClientStore clientStore;
        private readonly IEventService events;
        private readonly ILogger<ExternalController> logger;

        public ExternalController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            ILogger<ExternalController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.interaction = interaction;
            this.clientStore = clientStore;
            this.events = events;
            this.logger = logger;
        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Challenge(string provider, string returnUrl)
        {
            if(string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            // validate returnUrl - either it is a valid OIDC URL or back to a local page
            if(Url.IsLocalUrl(returnUrl) == false && interaction.IsValidReturnUrl(returnUrl) == false)
            {
                // user might have clicked on a malicious link - should be logged
                throw new Exception("invalid return URL");
            }

            if(AccountOptions.WindowsAuthenticationSchemeName == provider)
            {
                // windows authentication needs special handling
                return await ProcessWindowsLoginAsync(returnUrl);
            }

            // start challenge and roundtrip the return URL and scheme 
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback)),
                Items =
                {
                    {"returnUrl", returnUrl},
                    {"scheme", provider},
                }
            };

            return Challenge(props, provider);
        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Callback()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if(result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            if(logger.IsEnabled(LogLevel.Debug))
            {
                var externalClaims = result.Principal.Claims.Select(c => $"{c.Type}: {c.Value}");
                logger.LogDebug("External claims: {@claims}", externalClaims);
            }

            // lookup our user and external provider info
            var (user, provider, providerUserId, claims) = await FindUserFromExternalProviderAsync(result);
            if(user == null)
            {
                // this might be where you might initiate a custom workflow for user registration
                // in this sample we don't show how that would be done, as our sample implementation
                // simply auto-provisions new external user
                user = await AutoProvisionUserAsync(provider, providerUserId, claims as Claim[] ?? claims.ToArray());
            }

            // this allows us to collect any additonal claims or properties
            // for the specific prtotocols used and store them in the local auth cookie.
            // this is typically used to store data needed for signout from those protocols.
            var additionalLocalClaims = new List<Claim>();
            var localSignInProps = new AuthenticationProperties();
            ProcessLoginCallbackForOidc(result, additionalLocalClaims, localSignInProps);

            // issue authentication cookie for user
            // we must issue the cookie maually, and can't use the SignInManager because
            // it doesn't expose an API to issue additional claims from the login workflow
            var principal = await signInManager.CreateUserPrincipalAsync(user);
            additionalLocalClaims.AddRange(principal.Claims);
            // TODO: Use Guid
            var name = principal.FindFirst(JwtClaimTypes.Name)?.Value ?? user.Id.ToString();
            await HttpContext.SignInAsync(user.Id.ToString(), name, provider, localSignInProps, additionalLocalClaims.ToArray());

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // retrieve return URL
            var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

            // check if external login is in the context of an OIDC request
            var context = await interaction.GetAuthorizationContextAsync(returnUrl);
            // TODO: Use Guid
            await events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id.ToString(), name, true, context?.ClientId));

            if(context != null)
            {
                if(await clientStore.IsPkceClientAsync(context.ClientId))
                {
                    // if the client is PKCE then we assume it's native, so this change in how to
                    // return the response is for better UX for the end user.
                    return View("Redirect", new RedirectViewModel {RedirectUrl = returnUrl});
                }
            }

            return Redirect(returnUrl);
        }

        private async Task<IActionResult> ProcessWindowsLoginAsync(string returnUrl)
        {
            // see if windows auth has already been requested and succeeded
            var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
            if(result?.Principal is WindowsPrincipal wp)
            {
                // we will issue the external cookie and then redirect the
                // user back to the external callback, in essence, treating windows
                // auth the same as any other external authentication mechanism
                var props = new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Callback"),
                    Items =
                    {
                        {"returnUrl", returnUrl},
                        {"scheme", AccountOptions.WindowsAuthenticationSchemeName},
                    }
                };

                var id = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
                id.AddClaim(new Claim(JwtClaimTypes.Subject, wp.FindFirst(ClaimTypes.PrimarySid).Value));
                id.AddClaim(new Claim(JwtClaimTypes.Name, wp.Identity.Name));

                await HttpContext.SignInAsync(
                    IdentityServerConstants.ExternalCookieAuthenticationScheme,
                    new ClaimsPrincipal(id),
                    props);
                return Redirect(props.RedirectUri);
            }

            // trigger windows auth
            // since windows auth don't support the redirect uri,
            // this URL is re-triggered when we call challenge
            return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
        }

        private async Task<(User user, string provider, string providerUserId, IEnumerable<Claim> claims)>
            FindUserFromExternalProviderAsync(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);

            var provider = result.Properties.Items["scheme"];
            var providerUserId = userIdClaim.Value;

            // find external user
            var user = await userManager.FindByLoginAsync(provider, providerUserId);

            return (user, provider, providerUserId, claims);
        }

        private async Task<User> AutoProvisionUserAsync(string provider, string providerUserId, Claim[] claims)
        {
            // create a list of claims that we want to transfer into our store
            var filtered = new List<Claim>();

            // user's display name
            var name = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value ??
                       claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            if(name != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, name));
            }
            else
            {
                var first = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value ??
                            claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
                var last = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value ??
                           claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
                if(first != null && last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, $"{first} {last}"));
                }
                else if(first != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first));
                }
                else if(last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, last));
                }
            }

            // email
            var email = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value ??
                        claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if(email != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Email, email));
            }

            var user = new User
            {
                UserName = Guid.NewGuid().ToString(),
            };
            var identityResult = await userManager.CreateAsync(user);
            if(!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

            if(filtered.Any())
            {
                identityResult = await userManager.AddClaimsAsync(user, filtered);
                if(!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
            }

            identityResult = await userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));
            if(!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

            return user;
        }

        private static void ProcessLoginCallbackForOidc(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if(sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for signout
            var idToken = externalResult.Properties.GetTokenValue("id_token");
            if(idToken != null)
            {
                localSignInProps.StoreTokens(new[] {new AuthenticationToken {Name = "id_token", Value = idToken}});
            }
        }
    }
}