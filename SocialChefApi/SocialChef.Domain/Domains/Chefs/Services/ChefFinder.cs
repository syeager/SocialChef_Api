using System.Threading.Tasks;
using LittleByte.Asp.Exceptions;
using Microsoft.EntityFrameworkCore;
using SocialChef.Business.Relational;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Chefs
{
    public interface IChefFinder
    {
        Task<Chef> FindByIdAsync(Chef.Guid id);
        Task<Chef> FindByUserAsync(User.Guid userId);
    }

    public class ChefFinder : IChefFinder
    {
        private readonly SqlDbContext sqlContext;

        public ChefFinder(SqlDbContext sqlContext)
        {
            this.sqlContext = sqlContext;
        }

        public async Task<Chef> FindByIdAsync(Chef.Guid id)
        {
            var chefDao = await sqlContext.Chefs.FindAsync(id.Value);
            if(chefDao == null)
            {
                throw new NotFoundException(typeof(Chef), id.Value);
            }

            return chefDao;
        }

        public async Task<Chef> FindByUserAsync(User.Guid userId)
        {
            var chefDao = await sqlContext.Chefs.FirstOrDefaultAsync(c => c.UserID == userId.Value);
            if(chefDao == null)
            {
                throw new NotFoundException(typeof(User), userId.Value);
            }

            return chefDao;
        }
    }
}