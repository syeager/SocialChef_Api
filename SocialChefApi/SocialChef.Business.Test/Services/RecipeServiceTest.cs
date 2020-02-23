using System;
using System.Linq;
using System.Threading.Tasks;
using LittleByte.Asp.Business;
using LittleByte.Asp.Exceptions;
using LittleByte.Asp.Test.Utilities;
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

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DbContextUtility.CreateCosmosInMemory(ref dbContext, (IOptions<CosmosOptions>)null);
        }

        [SetUp]
        public void SetUp()
        {
            dbContext.EnsureRecreated();
            testObj = new RecipeService(dbContext);
        }

        [Test]
        public void Create_NullSteps_ThrowBadRequest()
        {
            var request = new CreateRecipeRequest("test", null);

            Assert.ThrowsAsync<BadRequestException>(() => testObj.CreateAsync(request));
        }

        [Test]
        public void Create_EmptySteps_ThrowBadRequest()
        {
            var request = new CreateRecipeRequest("test", Array.Empty<string>());

            Assert.ThrowsAsync<BadRequestException>(() => testObj.CreateAsync(request));
        }

        [Test]
        public async Task Create_HasSteps_CreateRecipe()
        {
            var request = new CreateRecipeRequest("test", new[] {"a", "b"});

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
            var entity = dbContext.AddAndSave(CreateTestRecipe());

            var found = await testObj.GetAsync(entity.ID);

            Assert.AreEqual(entity.ID, found.ID);
        }

        [Test]
        public void Delete_None_ThrowNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => testObj.DeleteAsync("abc"));
        }

        [Test]
        public async Task Delete_Exists_DeleteRecipe()
        {
            var entity = dbContext.AddAndSave(CreateTestRecipe());

            await testObj.DeleteAsync(entity.ID);

            Assert.False(dbContext.Recipes.Any());
        }

        [Test]
        public async Task GetAll_None_ReturnEmpty()
        {
            var response = await testObj.GetAsync(new PageRequest());

            Assert.AreEqual(0, response.Count);
        }

        [Test]
        public async Task GetAll_FullPage_ReturnFullResults()
        {
            const int pageSize = 2;
            (pageSize * 2).Do(i => dbContext.AddAndSave(CreateTestRecipe(i.ToString())));

            var request = new PageRequest(pageSize, 1);

            var response = await testObj.GetAsync(request);

            Assert.AreEqual(pageSize, response.Count);
            for(var i = 0; i < pageSize; i++)
            {
                Assert.AreEqual((i + pageSize).ToString(), response[i].Name);
            }
        }

        [Test]
        public async Task GetAll_PartialPage_ReturnPartialResults()
        {
            const int pageSize = 2;
            pageSize.Do(i => dbContext.AddAndSave(CreateTestRecipe(i.ToString())));

            var request = new PageRequest(pageSize + 1);

            var response = await testObj.GetAsync(request);

            Assert.AreEqual(pageSize, response.Count);
        }

        private static Recipe CreateTestRecipe(string name = "test") => new Recipe(name, Array.Empty<string>());
    }
}