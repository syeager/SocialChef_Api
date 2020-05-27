using Microsoft.EntityFrameworkCore;

namespace SocialChef.Domain.Relational
{
    public class SqlDbContext : DbContext
    {
        public DbSet<ChefDao> Chefs { get; set; } = null!;
        public DbSet<RecipeSummaryDao> RecipeSummaries { get; set; } = null!;

        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options) {}
    }
}