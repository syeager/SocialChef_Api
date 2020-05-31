using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Serilog;

namespace SocialChef.Identity
{
    public static class SeedData
    {
        public static void EnsureSeedData(ConfigurationDbContext configurationDbContext)
        {
            AddClients(configurationDbContext);
            AddIDs(configurationDbContext);
            AddResources(configurationDbContext);
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
    }
}