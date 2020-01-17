using System.Linq;
using System.Threading.Tasks;
using LittleByte.Asp.Test.Utility;
using NUnit.Framework;
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
            DbContextUtility.CreateCosmosInMemory(ref dbContext);

            testObj = new RecipeService(dbContext);
        }

        [TestCase]
        public async Task Create_Valid_CreateRecipe()
        {
            var dto = await testObj.CreateAsync("test");

            Assert.AreEqual(1, dbContext.Recipes.Count());
            Assert.IsNotNull(dto);
        }
    }
}