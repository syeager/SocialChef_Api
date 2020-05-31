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
                new Client
                {
                    ClientId = "spa-local",
                    ClientName = "SPA Client Local",
                    ClientUri = "http://localhost:8080",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =
                    {
                        "http://localhost:8080",
                        "http://localhost:8080/callback",
                        "http://localhost:8080/silent",
                        "http://localhost:8080/popup",
                    },

                    PostLogoutRedirectUris = {"http://localhost:8080"},
                    AllowedCorsOrigins = {"http://localhost:8080"},

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        Api1
                    }
                },
                new Client
                {
                    ClientId = "spa",
                    ClientName = "SPA Client",
                    ClientUri = "https://socialchefweb.z5.web.core.windows.net",

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =
                    {
                        "https://socialchefweb.z5.web.core.windows.net",
                        "https://socialchefweb.z5.web.core.windows.net/callback",
                        "https://socialchefweb.z5.web.core.windows.net/silent",
                        "https://socialchefweb.z5.web.core.windows.net/popup",
                    },

                    PostLogoutRedirectUris = {"https://socialchefweb.z5.web.core.windows.net"},
                    AllowedCorsOrigins = {"https://socialchefweb.z5.web.core.windows.net"},

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