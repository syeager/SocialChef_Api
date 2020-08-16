using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SocialChef.Domain.Relational
{
    public class SqlDbContext : ApiAuthorizationDbContext<UserDao>
    {
        public DbSet<ChefDao> Chefs { get; set; } = null!;
        public DbSet<RecipeSummaryDao> RecipeSummaries { get; set; } = null!;

        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options, null) {
            
        }

        public SqlDbContext(DbContextOptions<SqlDbContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions) {}
    }
}