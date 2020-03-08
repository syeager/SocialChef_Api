using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Serilog;
using SocialChef.Data.User.Models;

namespace SocialChef.Identity
{
    public static class SeedData
    {
        public static void EnsureSeedData(UserManager<User> userManager, ConfigurationDbContext configurationDbContext)
        {
            AddClients(configurationDbContext);
            AddIDs(configurationDbContext);
            AddResources(configurationDbContext);
            AddUsers(userManager);
        }

        private static void AddClients(ConfigurationDbContext context)
        {
            if(!context.Clients.Any())
            {
                Log.Debug("Clients being populated");
                foreach(var client in Config.Clients.ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }
            else
            {
                Log.Debug("Clients already populated");
            }
        }

        private static void AddIDs(ConfigurationDbContext context)
        {
            if(!context.IdentityResources.Any())
            {
                Log.Debug("IdentityResources being populated");
                foreach(var resource in Config.Ids.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
            else
            {
                Log.Debug("IdentityResources already populated");
            }
        }

        private static void AddResources(ConfigurationDbContext context)
        {
            if(!context.ApiResources.Any())
            {
                Log.Debug("ApiResources being populated");
                foreach(var resource in Config.Apis.ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
            else
            {
                Log.Debug("ApiResources already populated");
            }
        }

        private static void AddUsers(UserManager<User> userManager)
        {
            var alice = userManager.FindByNameAsync("alice").Result;
            if(alice == null)
            {
                alice = new User
                {
                    UserName = "alice"
                };
                var result = userManager.CreateAsync(alice, "Pass123$").Result;
                if(!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userManager.AddClaimsAsync(alice, new[]
                {
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServerConstants.ClaimValueTypes.Json)
                }).Result;
                if(!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("alice created");
            }
            else
            {
                Log.Debug("alice already exists");
            }

            var bob = userManager.FindByNameAsync("bob").Result;
            if(bob == null)
            {
                bob = new User
                {
                    UserName = "bob"
                };
                var result = userManager.CreateAsync(bob, "Pass123$").Result;
                if(!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userManager.AddClaimsAsync(bob, new[]
                {
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                    new Claim(JwtClaimTypes.Address, @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }", IdentityServerConstants.ClaimValueTypes.Json),
                    new Claim("location", "somewhere")
                }).Result;
                if(!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("bob created");
            }
            else
            {
                Log.Debug("bob already exists");
            }
        }
    }
}