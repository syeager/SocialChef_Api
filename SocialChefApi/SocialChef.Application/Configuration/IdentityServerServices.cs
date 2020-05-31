using IdentityServer4.AccessTokenValidation;
using LittleByte.Asp.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocialChef.Domain.Identity;

namespace SocialChef.Application.Configuration
{
    public static class IdentityServerServices
    {
        public static void AddIdentityServer(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            var identityOptions = configuration.GetSection<IdentityOptions>(IdentityOptions.Key);

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = identityOptions.Address;
                    options.ApiName = "api1";
                    options.SupportedTokens = SupportedTokens.Jwt;
                    options.RequireHttpsMetadata = !environment.IsDevelopment();
                });
        }
    }
}