using System.Linq;
using System.Threading.Tasks;
using LittleByte.Asp.Database;
using LittleByte.Asp.Exceptions;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Document;
using SocialChef.Domain.Relational;

namespace SocialChef.Domain.Recipes
{
    public interface IRecipeCreator
    {
        Task<Recipe> CreateAsync(Recipe recipe);
    }

    public class RecipeCreator : IRecipeCreator
    {
        private readonly CosmosContext documentContext;
        private readonly SqlDbContext sqlContext;

        public RecipeCreator(CosmosContext documentContext, SqlDbContext sqlContext)
        {
            this.documentContext = documentContext;
            this.sqlContext = sqlContext;
        }

        public async Task<Recipe> CreateAsync(Recipe recipe)
        {
            var cheDao = await sqlContext.Chefs.FindAsync(recipe.ChefID.Value);
            if(cheDao == null)
            {
                throw new NotFoundException(typeof(Chef), recipe.ChefID.Value);
            }

            RecipeDao recipeDao = recipe;
            recipeDao.ID = DomainGuid<Recipe>.Empty;

            await documentContext.AddAndSaveAsync(recipeDao);

            var stepCount = recipe.Sections.Sum(s => s.Steps.Count);
            var recipeSummaryDao = new RecipeSummaryDao(recipeDao.ID, recipeDao.Name, recipeDao.ChefID, stepCount);
            await sqlContext.AddAndSaveAsync(recipeSummaryDao);

            return recipeDao;
        }
    }
}