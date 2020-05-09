using System;
using System.Threading.Tasks;
using LittleByte.Asp.Business;
using LittleByte.Asp.Test.Database;
using LittleByte.Asp.Test.Utilities;
using LittleByte.Domain.Test.Utilities;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SocialChef.Business.Document;
using SocialChef.Business.Relational;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Services
{
    public class RecipeFinderTest
    {
        private RecipeFinder testObj;
        private CosmosContext cosmosContext;
        private SqlDbContext sqlContext;

        private ChefDao chefDao;

        [SetUp]
        public void SetUp()
        {
            DbContextUtility.CreateCosmosInMemory(ref cosmosContext, (IOptions<CosmosOptions>)null);
            DbContextUtility.CreateInMemory(ref sqlContext);

            chefDao = new ChefDao(Guid.NewGuid(), "name");
            sqlContext.AddAndSave(chefDao);

            testObj = new RecipeFinder(cosmosContext, sqlContext);
        }

        [Test]
        public void FindById_NoRecipe_ThrowNotFound()
        {
            var id = new Recipe.Guid(Guid.Empty);

            AssertExtension.ThrowsNotFoundAsync<Recipe>(() => testObj.FindByIdAsync(id), id.Value);
        }

        [Test]
        public async Task FindById_HasRecipe_ReturnRecipe()
        {
            var recipe = RecipeTestCreator.RecipeWithNewGuid();
            cosmosContext.AddAndSave((RecipeDao)recipe.Model!);

            var result = await testObj.FindByIdAsync(recipe.Model!.ID);

            Assert.AreEqual(recipe.Model!.ID, result.ID);
        }

        [Test]
        public void FindByChef_NoChef_ThrowNotFound()
        {
            var chefID = new Chef.Guid(Guid.Empty);
            var page = new PageRequest();

            AssertExtension.ThrowsNotFoundAsync<Chef>(() => testObj.FindByChefAsync(chefID, page), chefID.Value);
        }

        [Test]
        public async Task FindByChef_NoRecipe_ReturnEmpty()
        {
            var chefID = new Chef.Guid(chefDao.ID);
            var page = new PageRequest();

            var result = await testObj.FindByChefAsync(chefID, page);

            Assert.IsEmpty(result.Results);
        }

        [Test]
        public async Task FindByChef_HasRecipes_ReturnResults()
        {
            var chefID = new Chef.Guid(chefDao.ID);
            var page = new PageRequest();

            AddRecipeToDb(chefID.Value);

            var result = await testObj.FindByChefAsync(chefID, page);

            Assert.IsNotEmpty(result.Results);
        }

        [Test]
        public async Task GetLatest_NoRecipes_ReturnEmpty()
        {
            var page = new PageRequest();

            var result = await testObj.GetLatest(page);

            Assert.IsEmpty(result.Results);
        }

        [Test]
        public async Task GetLatest_HasRecipes_ReturnsResults()
        {
            var page = new PageRequest();
            AddRecipeToDb(Guid.NewGuid());

            var result = await testObj.GetLatest(page);

            Assert.IsNotEmpty(result.Results);
        }

        private void AddRecipeToDb(Guid chefId)
        {
            RecipeDao recipe = RecipeTestCreator.RecipeWithNewGuid(chefId).GetModelOrThrow();
            cosmosContext.AddAndSave(recipe);

            var chefRecipe = new ChefRecipe(chefId, recipe.ID);
            sqlContext.AddAndSave(chefRecipe);
        }
    }
}