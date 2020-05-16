using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SocialChef.Domain.Document
{
    public class CosmosContext : DbContext
    {
        private readonly CosmosOptions? options;

        public DbSet<RecipeDao> Recipes { get; set; } = null!;

        private CosmosContext(IOptions<CosmosOptions> options)
        {
            this.options = options.Value;
        }

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
            modelBuilder.Entity<RecipeDao>()
                .OwnsMany(r => r.Sections, sectionBuilder =>
                {
                    sectionBuilder.OwnsMany(s => s.Steps, stepBuilder =>
                    {
                        stepBuilder.WithOwner();
                        stepBuilder.HasKey(nameof(StepDao.Index));
                    });
                });
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