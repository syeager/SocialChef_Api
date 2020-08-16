using System;
using System.Threading.Tasks;
using LittleByte.Asp.Exceptions;
using LittleByte.Asp.Test.Database;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SocialChef.Domain.Document;
using SocialChef.Domain.Recipes;
using SocialChef.Domain.Relational;
using SocialChef.Domain.Test.Utilities;

namespace SocialChef.Domain.Test.Domains.Recipes.Services
{
    public class RecipeCreatorTest
    {
        private RecipeCreator testObj;
        private CosmosContext cosmosContext;
        private SqlDbContext sqlContext;

        private Guid chefId;

        [SetUp]
        public void SetUp()
        {
            DbContextFactory.BuildCosmos(ref cosmosContext);
            DbContextFactory.BuildSql(ref sqlContext);

            testObj = new RecipeCreator(cosmosContext, sqlContext);

            chefId = sqlContext.AddAndSave(new ChefDao(Guid.Empty, "name")).ID;
        }

        [Test]
        public async Task Create_Valid_ReturnDto()
        {
            var request = RecipeTestCreator.RecipeWithEmptyGuid(chefId).GetModelOrThrow();

            var response = await testObj.CreateAsync(request!);

            Assert.NotNull(response);
            Assert.AreNotEqual(request.ID.Value, response.ID.Value);
        }

        [Test]
        public async Task Create_Valid_RecipeSummaryCreated()
        {
            var request = RecipeTestCreator.RecipeWithEmptyGuid(chefId).GetModelOrThrow();

            var response = await testObj.CreateAsync(request!);
            var recipeSummary = await sqlContext.RecipeSummaries.FirstAsync();

            Assert.AreEqual(response.ID.Value, recipeSummary.ID);
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