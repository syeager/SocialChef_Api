using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace SocialChef.Persistence
{
    public class CosmosContext: DbContext
    {
        private readonly CosmosConfig config;

        public DbSet<Recipe> Recipes { get; set; } = null!;

        public CosmosContext(IOptions<CosmosConfig> config)
        {
            this.config = config.Value;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(config.Url, config.Key, config.DatabaseName);
        }
    }
}