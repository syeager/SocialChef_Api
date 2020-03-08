using Microsoft.EntityFrameworkCore;
using SocialChef.Business.Relational.Models;

namespace SocialChef.Business.Relational.Contexts
{
    public class SqlDbContext : DbContext
    {
        public DbSet<Chef> Chefs { get; set; } = null!;
        public DbSet<ChefRecipe> ChefRecipes { get; set; } = null!;

        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChefRecipe>().HasKey(nameof(ChefRecipe.ChefID), nameof(ChefRecipe.RecipeID));
        }
    }
}