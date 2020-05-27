using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using LittleByte.Asp.Exceptions;
using Microsoft.EntityFrameworkCore;
using SocialChef.Domain.Identity;
using SocialChef.Domain.Relational;

namespace SocialChef.Domain.Chefs
{
    public interface IChefFinder
    {
        Task<Chef> FindByIdAsync(Guid id);
        Task<Chef> FindByUserAsync(DomainGuid<User> userId);
        Task<IReadOnlyList<Chef>> FindByIdsAsync(IReadOnlyList<Guid> chefIds);
    }

    public class ChefFinder : IChefFinder
    {
        private readonly SqlDbContext sqlContext;

        public ChefFinder(SqlDbContext sqlContext)
        {
            this.sqlContext = sqlContext;
        }

        public Task<Chef> FindByIdAsync(Guid id)
        {
            var chefId = new DomainGuid<Chef>(id);
            return FindByIdAsync(chefId);
        }

        internal async Task<Chef> FindByIdAsync(DomainGuid<Chef> id)
        {
            var chefDao = await sqlContext.Chefs.FindAsync(id.Value);
            if(chefDao == null)
            {
                throw new NotFoundException(typeof(Chef), id.Value);
            }

            return chefDao;
        }

        public async Task<Chef> FindByUserAsync(DomainGuid<User> userId)
        {
            var chefDao = await sqlContext.Chefs.FirstOrDefaultAsync(c => c.UserID == userId.Value);
            if(chefDao == null)
            {
                throw new NotFoundException(typeof(User), userId.Value);
            }

            return chefDao;
        }

        public async Task<IReadOnlyList<Chef>> FindByIdsAsync(IReadOnlyList<Guid> chefIds)
        {
            var distinctChefIds = chefIds.Distinct().ToImmutableArray();

            var chefs = new List<Chef>(distinctChefIds.Length);
            foreach(var chefId in distinctChefIds)
            {
                var chef = await FindByIdAsync(chefId);
                chefs.Add(chef);
            }

            return chefs;
        }
    }
}