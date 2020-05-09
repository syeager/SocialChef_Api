using System.Linq;
using System.Threading.Tasks;
using LittleByte.Asp.Business;
using LittleByte.Asp.Database;
using LittleByte.Asp.Exceptions;
using Microsoft.EntityFrameworkCore;
using SocialChef.Business.Document;
using SocialChef.Business.Relational;

namespace SocialChef.Domain.Recipes
{
    public interface IRecipeFinder
    {
        Task<Recipe> FindByIdAsync(Recipe.Guid id);
        Task<PageResponse<Recipe>> FindByChefAsync(Chef.Guid id, PageRequest page);
        Task<PageResponse<Recipe>> GetLatest(PageRequest page);
    }

    public class RecipeFinder : IRecipeFinder
    {
        private readonly CosmosContext cosmosContext;
        private readonly SqlDbContext sqlContext;

        public RecipeFinder(CosmosContext cosmosContext, SqlDbContext sqlContext)
        {
            this.cosmosContext = cosmosContext;
            this.sqlContext = sqlContext;
        }

        public async Task<Recipe> FindByIdAsync(Recipe.Guid id)
        {
            var entity = await FromRecipes().FirstOrDefaultAsync(r => r.ID == id.Value);
            if(entity == null)
            {
                throw new NotFoundException(typeof(Recipe), id.Value);
            }

            return entity;
        }

        public async Task<PageResponse<Recipe>> FindByChefAsync(Chef.Guid id, PageRequest page)
        {
            var chefDao = await sqlContext.Chefs.FindAsync(id.Value);
            if(chefDao == null)
            {
                throw new NotFoundException(typeof(Chef), id.Value);
            }

            var recipeDaoPage = await FromRecipes()
                .Where(r => r.ChefID == chefDao.ID)
                .GetPagedAsync(page.PageSize, page.Page);

            var recipePage = recipeDaoPage.CastResults(r => (Recipe)r);
            return recipePage;
        }

        public async Task<PageResponse<Recipe>> GetLatest(PageRequest page)
        {
            var recipeDaoPage = await FromRecipes()
                .GetPagedAsync(page.PageSize, page.Page);

            var recipePage = recipeDaoPage.CastResults(r => (Recipe)r);
            return recipePage;
        }

        private IQueryable<RecipeDao> FromRecipes()
        {
            return cosmosContext.Recipes
                .Include(r => r.Sections).ThenInclude(s => s.Steps).AsNoTracking();
        }
    }
}