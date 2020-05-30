using System;
using System.Security.Cryptography;
using IdentityServer4;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace SocialChef.Identity.Extensions
{
    public static class IdentityServerBuilderExtention
    {
        public static void AddSigningCredentials(this IIdentityServerBuilder builder, IConfiguration configuration)
        {
            const string configKey = "SigningCredentials";
            var privateKey = configuration[configKey];

            var privateKeyBytes = Convert.FromBase64String(privateKey);
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
            var securityKey = new RsaSecurityKey(rsa);
            builder.AddSigningCredential(securityKey, IdentityServerConstants.RsaSigningAlgorithm.RS256);
        }
    }
}