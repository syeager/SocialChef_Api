using System;
using System.Collections.Generic;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace SocialChef.Domain.Identity.Config
{
    public static class Clients
    {
        public static IEnumerable<OpenIddictApplicationDescriptor> Get()
        {
            return new[]
            {
                new OpenIddictApplicationDescriptor
                {
                    ClientId = "postman",
                    ClientSecret = "secret",
                    ConsentType = ConsentTypes.Implicit,
                    DisplayName = "Postman",
                    PostLogoutRedirectUris =
                    {
                        new Uri("https://www.getpostman.com")
                    },
                    RedirectUris =
                    {
                        new Uri("https://www.getpostman.com/oauth2/callback")
                    },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "demo_api"
                    }
                }
            };
        }
    }
}