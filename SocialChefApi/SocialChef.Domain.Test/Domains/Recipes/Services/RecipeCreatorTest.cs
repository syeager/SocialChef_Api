using System;
using System.Threading.Tasks;
using LittleByte.Asp.Exceptions;
using LittleByte.Asp.Test.Database;
using LittleByte.Domain.Test.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SocialChef.Domain.Document;
using SocialChef.Domain.Relational;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Services
{
    public class RecipeCreatorTest
    {
        private RecipeCreator testObj;
        private CosmosContext documentContext;
        private SqlDbContext sqlContext;

        private Guid chefId;

        [SetUp]
        public void SetUp()
        {
            DbContextUtility.CreateCosmosInMemory(ref documentContext, (IOptions<CosmosOptions>)null);
            DbContextUtility.CreateInMemory(ref sqlContext);

            testObj = new RecipeCreator(documentContext, sqlContext);

            chefId = sqlContext.AddAndSave(new ChefDao(Guid.Empty, "name")).ID;
        }

        [Test]
        public async Task Create_Valid_ReturnDto()
        {
            var request = RecipeTestCreator.RecipeWithEmptyGuid(chefId).GetModelOrThrow();

            var response = await testObj.CreateAsync(request!);
            var chefRecipe = await sqlContext.ChefRecipes.FirstAsync();
            var count = await sqlContext.ChefRecipes.CountAsync();

            Assert.AreEqual(1, count);
            Assert.NotNull(response);
            Assert.AreNotEqual(request.ID.Value, response.ID.Value);
            Assert.AreEqual(response.ID.Value, chefRecipe.RecipeID);
        }

        [Test]
        public async Task Create_NonEmptyID_ReturnDtoWithNewId()
        {
            var request = RecipeTestCreator.RecipeWithNewGuid(chefId).GetModelOrThrow();

            var response = await testObj.CreateAsync(request!);

            Assert.NotNull(response);
            Assert.AreNotEqual(request.ID.Value, response.ID.Value);
        }

        [Test]
        public void Create_NoChef_ThrowNotFound()
        {
            var request = RecipeTestCreator.RecipeWithEmptyGuid(Guid.NewGuid()).GetModelOrThrow();

            Assert.ThrowsAsync<NotFoundException>(() => testObj.CreateAsync(request!));
        }
    }
}