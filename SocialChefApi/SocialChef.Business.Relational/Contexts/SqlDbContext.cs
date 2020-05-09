using Microsoft.EntityFrameworkCore;

namespace SocialChef.Business.Relational
{
    public class SqlDbContext : DbContext
    {
        public DbSet<ChefDao> Chefs { get; set; } = null!;
        public DbSet<ChefRecipe> ChefRecipes { get; set; } = null!;

        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChefRecipe>().HasKey(nameof(ChefRecipe.ChefID), nameof(ChefRecipe.RecipeID));
        }
    }
}