using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SocialChef.Persistence
{
    public class CosmosContext : DbContext
    {
        private readonly CosmosOptions options;

        public DbSet<Recipe> Recipes { get; set; } = null!;

        public CosmosContext(IOptions<CosmosOptions> options)
        {
            this.options = options.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(options.Url, options.Key, options.DatabaseName);
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