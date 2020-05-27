using System;
using System.Linq;
using System.Threading.Tasks;
using LittleByte.Asp.Business;
using LittleByte.Asp.Database;
using LittleByte.Asp.Exceptions;
using Microsoft.EntityFrameworkCore;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Document;
using SocialChef.Domain.Relational;

namespace SocialChef.Domain.Recipes
{
    public interface IRecipeFinder
    {
        Task<Recipe> FindByIdAsync(Guid id);
        Task<PageResponse<RecipeSummary>> FindByChefAsync(Guid id, PageRequest page);
        Task<PageResponse<RecipeSummary>> GetLatest(PageRequest page);
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

        public Task<Recipe> FindByIdAsync(Guid id)
        {
            return FindByIdAsync(new DomainGuid<Recipe>(id));
        }

        internal async Task<Recipe> FindByIdAsync(DomainGuid<Recipe> id)
        {
            var entity = await FromRecipes().FirstOrDefaultAsync(r => r.ID == id.Value);
            if(entity == null)
            {
                throw new NotFoundException(typeof(Recipe), id.Value);
            }

            return entity;
        }

        public Task<PageResponse<RecipeSummary>> FindByChefAsync(Guid id, PageRequest page)
        {
            var chefId = new DomainGuid<Chef>(id);
            return FindByChefAsync(chefId, page);
        }

        internal async Task<PageResponse<RecipeSummary>> FindByChefAsync(DomainGuid<Chef> id, PageRequest page)
        {
            var chefDao = await sqlContext.Chefs.FindAsync(id.Value);
            if(chefDao == null)
            {
                throw new NotFoundException(typeof(Chef), id.Value);
            }

            var recipeSummaryDaoPage = await FromRecipeSummaries()
                .Where(rs => rs.ChefId == chefDao.ID)
                .GetPagedAsync(page.PageSize, page.Page);

            var recipeSummaryPage = recipeSummaryDaoPage.CastResults(r => (RecipeSummary)r);
            return recipeSummaryPage;
        }

        public async Task<PageResponse<RecipeSummary>> GetLatest(PageRequest page)
        {
            var recipeSummaryDaoPage = await FromRecipeSummaries()
                .GetPagedAsync(page.PageSize, page.Page);

            var recipeSummaryPage = recipeSummaryDaoPage.CastResults(r => (RecipeSummary)r);
            return recipeSummaryPage;
        }

        private IQueryable<RecipeDao> FromRecipes()
        {
            return cosmosContext.Recipes
                .Include(r => r.Sections).ThenInclude(s => s.Steps).AsNoTracking();
        }

        private IQueryable<RecipeSummaryDao> FromRecipeSummaries()
        {
            return sqlContext.RecipeSummaries.AsNoTracking();
        }
    }
}