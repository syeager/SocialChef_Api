﻿using IdentityServer4.EntityFramework.Options;
using LittleByte.Asp.Test.Database;
using Microsoft.Extensions.Options;
using SocialChef.Domain.Document;
using SocialChef.Domain.Relational;

namespace SocialChef.Domain.Test.Utilities
{
    public static class DbContextFactory
    {
        public static void BuildSql(ref SqlDbContext dbContext)
        {
            DbContextUtility.CreateInMemoryWithOptions(ref dbContext, Options.Create(new OperationalStoreOptions()));
        }

        public static void BuildCosmos(ref CosmosContext dbContext)
        {
            DbContextUtility.CreateInMemoryWithOptions(ref dbContext, (IOptions<CosmosOptions>)null);
        }
    }
}