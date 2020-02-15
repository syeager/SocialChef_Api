using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace SocialChef.Identity
{
    public static class Config
    {
        private const string Api1 = "api1";

        public static IEnumerable<IdentityResource> Ids =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiResource> Apis =>
            new[]
            {
                new ApiResource(Api1, "My API #1")
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                    ClientId = "postman-api",
                    ClientName = "Postman Test Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,
                    RedirectUris = {"https://www.getpostman.com/oauth2/callback"},
                    //NOTE: This link needs to match the link from the presentation layer - oidc-client
                    //      otherwise IdentityServer won't display the link to go back to the site
                    PostLogoutRedirectUris = {"https://www.getpostman.com"},
                    AllowedCorsOrigins = {"https://www.getpostman.com"},
                    EnableLocalLogin = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        Api1,
                    },
                    ClientSecrets = new List<Secret> {new Secret("SomeValue".Sha256())}
                },
                // client credentials flow client
                new Client
                {
                    ClientId = "client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256())},

                    AllowedScopes = {Api1}
                },

                // MVC client using code flow + pkce
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequirePkce = true,
                    ClientSecrets = {new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256())},

                    RedirectUris = {"http://localhost:5002/signin-oidc"},
                    FrontChannelLogoutUri = "http://localhost:5002/signout-oidc",
                    PostLogoutRedirectUris = {"http://localhost:5002/signout-callback-oidc"},

                    AllowOfflineAccess = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        Api1
                    }
                },

                // SPA client using code flow + pkce
                new Client
                {
                    ClientId = "spa",
                    ClientName = "SPA Client",
                    ClientUri = "http://identityserver.io",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =
                    {
                        "http://localhost:5002/index.html",
                        "http://localhost:5002/callback.html",
                        "http://localhost:5002/silent.html",
                        "http://localhost:5002/popup.html",
                    },

                    PostLogoutRedirectUris = {"http://localhost:5002/index.html"},
                    AllowedCorsOrigins = {"http://localhost:5002"},

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        Api1
                    }
                }
            };
    }
}