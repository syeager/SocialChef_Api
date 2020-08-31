using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SocialChef.Domain.Relational
{
    public class SqlDbContext : IdentityDbContext<UserDao>
    {
        public DbSet<ChefDao> Chefs { get; set; } = null!;
        public DbSet<RecipeSummaryDao> RecipeSummaries { get; set; } = null!;

        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options) {}
    }
}