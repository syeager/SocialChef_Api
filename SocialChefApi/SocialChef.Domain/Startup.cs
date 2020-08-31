using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using LittleByte.Asp.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Document;
using SocialChef.Domain.Identity;
using SocialChef.Domain.Recipes;
using SocialChef.Domain.Relational;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace SocialChef.Domain
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            AddServices(services);
            AddDataStores(services, configuration);
            AddOidc(services);

            services.AddHostedService<OidcBootService>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<HttpClient>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IAuthorizationService, AuthorizationService>();

            services.AddTransient<IChefCreator, ChefCreator>();
            services.AddTransient<IChefFinder, ChefFinder>();

            services.AddTransient<IRecipeCreator, RecipeCreator>();
            services.AddTransient<IRecipeFinder, RecipeFinder>();

            services.AddIdentity<UserDao, IdentityRole>()
                .AddEntityFrameworkStores<SqlDbContext>();
        }

        private static void AddDataStores(IServiceCollection services, IConfiguration configuration)
        {
            AddCosmosDB(services, configuration);
            AddSqlDB(services, configuration);
        }

        private static void AddCosmosDB(IServiceCollection services, IConfiguration configuration)
        {
            var cosmosOptions = GetCosmosOptions(services, configuration);
            services.AddDbContext<CosmosContext>();
            services.AddHealthChecks().AddDbContextCheck<CosmosContext>("CosmosDB", customTestQuery: HealthCheck);
            CosmosContext.Create(cosmosOptions).GetAwaiter();

            Task<bool> HealthCheck(CosmosContext context, CancellationToken token)
            {
                return context.CanCosmosConnectAsync(cosmosOptions.DatabaseName, cosmosOptions.ContainerName, token);
            }
        }

        private static CosmosOptions GetCosmosOptions(IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(CosmosOptions.OptionsKey);
            services.Configure<CosmosOptions>(section);

            var options = section.Get<CosmosOptions>();
            return options;
        }

        private static void AddSqlDB(IServiceCollection services, IConfiguration configuration)
        {
            var sqlConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddHealthChecks().AddDbContextCheck<SqlDbContext>("SqlDB");
            services.AddDbContext<SqlDbContext>(options =>
            {
                options.UseSqlServer(sqlConnectionString);
                options.UseOpenIddict();
            });
        }

        private static void AddOidc(IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
            });

            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<SqlDbContext>();
                })
                .AddServer(options =>
                {
                    options
                        .SetAuthorizationEndpointUris("/connect/authorize")
                        .SetTokenEndpointUris("/connect/token")
                        .SetLogoutEndpointUris("/connect/logout")
                        .SetIntrospectionEndpointUris("/connect/introspect")
                        .SetUserinfoEndpointUris("/connect/userinfo");

                    options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);

                    options
                        .AllowImplicitFlow()
                        .AllowAuthorizationCodeFlow();

                    // TODO: Make prod certs.
                    options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();

                    options
                        .UseAspNetCore()
                        .EnableAuthorizationEndpointPassthrough()
                        .EnableUserinfoEndpointPassthrough()
                        .EnableStatusCodePagesIntegration()
                        // TODO: Remove.
                        .DisableTransportSecurityRequirement();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });
        }
    }
}