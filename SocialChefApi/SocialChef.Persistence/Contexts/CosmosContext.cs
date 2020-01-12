using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SocialChef.Persistence
{
    [UsedImplicitly]
    public class CosmosContext: DbContext
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
    }
}