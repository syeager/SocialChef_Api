using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SocialChef.Persistence.Models;

namespace SocialChef.Persistence.Contexts
{
    public class CosmosContext : DbContext
    {
        private readonly CosmosOptions? options;

        public DbSet<Recipe> Recipes { get; set; } = null!;

        private CosmosContext(IOptions<CosmosOptions> options)
        {
            this.options = options.Value;
        }

        [UsedImplicitly]
        public CosmosContext(DbContextOptions<CosmosContext> contextOptions, IOptions<CosmosOptions>? options)
            : base(contextOptions)
        {
            this.options = options?.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(options != null)
            {
                optionsBuilder.UseCosmos(options.Url, options.Key, options.DatabaseName);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>().OwnsMany(o => o.Steps);
        }

        public static async Task Create(CosmosOptions options)
        {
            try
            {
                await using var context = new CosmosContext(new OptionsWrapper<CosmosOptions>(options));
                var created = await context.Database.EnsureCreatedAsync();
                Console.Write($"Setup Cosmos: {(created ? "Created" : "Exists")}");
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}