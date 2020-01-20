using System.Linq;
using System.Threading.Tasks;
using LittleByte.Asp.Exceptions;
using LittleByte.Asp.Test.Utility;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SocialChef.Business.Requests;
using SocialChef.Business.Services;
using SocialChef.Persistence;

namespace SocialChef.Business.Test.Services
{
    public class RecipeServiceTest
    {
        private RecipeService testObj;
        private CosmosContext dbContext;

        [SetUp]
        public void SetUp()
        {
            DbContextUtility.CreateCosmosInMemory(ref dbContext, (IOptions<CosmosOptions>)null);

            testObj = new RecipeService(dbContext);
        }

        [TestCase]
        public async Task Create_Valid_CreateRecipe()
        {
            var request = new CreateRecipeRequest("test");

            var dto = await testObj.CreateAsync(request);

            Assert.AreEqual(1, dbContext.Recipes.Count());
            Assert.IsNotNull(dto);
        }

        [Test]
        public void Get_None_ThrowNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => testObj.GetAsync("abc"));
        }

        [Test]
        public async Task Get_Exists_ReturnRecipe()
        {
            var entity = dbContext.AddAndSave(new Recipe("test"));

            var found = await testObj.GetAsync(entity.ID);

            Assert.AreEqual(entity.ID, found.ID);
        }
    }
}