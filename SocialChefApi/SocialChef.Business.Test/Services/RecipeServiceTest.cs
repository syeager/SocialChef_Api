using System;
using System.Linq;
using System.Threading.Tasks;
using LittleByte.Asp.Business;
using LittleByte.Asp.Exceptions;
using LittleByte.Asp.Test.Database;
using LittleByte.Asp.Test.Utilities;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using SocialChef.Business.Document.Contexts;
using SocialChef.Business.Document.Models;
using SocialChef.Business.Document.Options;
using SocialChef.Business.DTOs;
using SocialChef.Business.Relational.Contexts;
using SocialChef.Business.Relational.Models;
using SocialChef.Business.Requests;
using SocialChef.Business.Services;

namespace SocialChef.Business.Test
{
    public class RecipeServiceTest
    {
        private RecipeService testObj;
        private CosmosContext documentContext;
        private SqlDbContext sqlContext;
        private IChefService chefService;

        private Chef chef;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DbContextUtility.CreateCosmosInMemory(ref documentContext, (IOptions<CosmosOptions>)null);
            DbContextUtility.CreateInMemory(ref sqlContext, "sql");
        }

        [SetUp]
        public void SetUp()
        {
            documentContext.EnsureRecreated();
            sqlContext.EnsureRecreated();

            chef = new Chef(Guid.NewGuid(), "bob");
            chefService = Substitute.For<IChefService>();
            chefService.ToDto(chef).Returns(new ChefDto(chef.ID, chef.Name));

            testObj = new RecipeService(documentContext, sqlContext, chefService);
        }

        [Test]
        public void Create_NullSteps_ThrowBadRequest()
        {
            var request = new CreateRecipeRequest(chef.ID, "test", null);

            Assert.ThrowsAsync<BadRequestException>(() => testObj.CreateAsync(request));
        }

        [Test]
        public void Create_NoChef_ThrowNotFound()
        {
            var request = new CreateRecipeRequest(Guid.Empty, "test", new[] {"a", "b"});

            Assert.ThrowsAsync<NotFoundException>(() => testObj.CreateAsync(request));
        }

        [Test]
        public void Create_EmptySteps_ThrowBadRequest()
        {
            var request = new CreateRecipeRequest(chef.ID, "test", Array.Empty<string>());

            Assert.ThrowsAsync<BadRequestException>(() => testObj.CreateAsync(request));
        }

        [Test]
        public async Task Create_Valid_CreateRecipe()
        {
            sqlContext.AddAndSave(chef);
            var request = new CreateRecipeRequest(chef.ID, "test", new[] {"a", "b"});

            var dto = await testObj.CreateAsync(request);

            Assert.AreEqual(1, documentContext.Recipes.Count());
            Assert.IsNotNull(dto);
        }

        [Test]
        public async Task Create_Valid_CreateChefRecipe()
        {
            sqlContext.AddAndSave(chef);
            var request = new CreateRecipeRequest(chef.ID, "test", new[] {"a", "b"});

            var dto = await testObj.CreateAsync(request);

            Assert.AreEqual(chef.ID, sqlContext.ChefRecipes.First().ChefID);
            Assert.AreEqual(dto.ID, sqlContext.ChefRecipes.First().RecipeID);
        }

        [Test]
        public async Task Get_Exists_ReturnRecipe()
        {
            AddAndReturnChef();
            var entity = documentContext.AddAndSave(CreateTestRecipe());

            var found = await testObj.GetAsync(entity.ID);

            Assert.AreEqual(entity.ID, found.ID);
            Assert.AreEqual(chef.ID, found.Chef.ID);
        }

        [Test]
        public async Task GetAll_None_ReturnEmpty()
        {
            var response = await testObj.GetAsync(new PageRequest());

            Assert.AreEqual(0, response.Results.Count);
        }

        [Test]
        public async Task GetAll_FullPage_ReturnFullResults()
        {
            AddAndReturnChef();
            const int pageSize = 2;
            (pageSize * 2).Do(i => documentContext.AddAndSave(CreateTestRecipe(i.ToString())));

            var request = new PageRequest(pageSize, 1);

            var response = await testObj.GetAsync(request);

            Assert.AreEqual(pageSize, response.Results.Count);
            for(var i = 0; i < pageSize; i++)
            {
                Assert.AreEqual((i + pageSize).ToString(), response.Results[i].Name);
                Assert.AreEqual(chef.ID, response.Results[i].Chef.ID);
            }
        }

        [Test]
        public async Task GetAll_PartialPage_ReturnPartialResults()
        {
            AddAndReturnChef();
            const int pageSize = 2;
            pageSize.Do(i => documentContext.AddAndSave(CreateTestRecipe(i.ToString())));

            var request = new PageRequest(pageSize + 1);

            var response = await testObj.GetAsync(request);

            Assert.AreEqual(pageSize, response.Results.Count);
        }

        [Test]
        public async Task GetAll_Valid_ReturnsCorrectPageCount()
        {
            AddAndReturnChef();
            const int pageSize = 2;
            (pageSize + 1).Do(i => documentContext.AddAndSave(CreateTestRecipe(i.ToString())));

            var request = new PageRequest(pageSize);

            var response = await testObj.GetAsync(request);

            Assert.That(response.TotalPages, Is.EqualTo(2));
        }

        [Test]
        public void GetChefRecipes_NoChef_ThrowNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => testObj.GetForChefAsync(chef.ID, new PageRequest()));
        }

        [Test]
        public async Task GetChefRecipes_HasChef_ReturnRecipes()
        {
            sqlContext.AddAndSave(chef);

            var recipe = new Recipe(chef.ID, "test", new[] {"a, b"});
            documentContext.AddAndSave(recipe);

            var response = await testObj.GetForChefAsync(chef.ID, new PageRequest());

            Assert.AreEqual(recipe.ID, response.Results[0].ID);
        }

        private Recipe CreateTestRecipe(string name = "test") => new Recipe(chef.ID, name, Array.Empty<string>());

        private void AddAndReturnChef()
        {
            sqlContext.AddAndSave(chef);
            chefService.GetChefAsync(chef.ID).Returns(new ChefDto(chef.ID, "bob"));
        }
    }
}