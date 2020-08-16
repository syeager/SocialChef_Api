using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SocialChef.Application.Controllers;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Test.Utilities;

namespace SocialChef.Application.Test
{
    public class ChefControllerTest
    {
        private ChefController testObj;
        private IChefFinder chefFinder;

        [SetUp]
        public void SetUp()
        {
            chefFinder = Substitute.For<IChefFinder>();

            testObj = new ChefController(chefFinder);
        }

        [Test]
        public async Task GetByUser_FoundChef_ReturnChef200()
        {
            var chef = CreateChef();
            chefFinder.FindByUserAsync(chef.UserId).Returns(chef);

            var response = await testObj.GetByUser(chef.UserId);

            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(chef.ID.Value, response.Data!.ID);
        }

        [Test]
        public async Task Get_FoundChef_ReturnChef200()
        {
            var chef = CreateChef();
            chefFinder.FindByIdAsync(chef.ID).Returns(chef);

            var response = await testObj.Get(chef.ID);

            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(chef.ID.Value, response.Data!.ID);
        }

        [Test]
        public async Task GetByBatch_NoChefs_ReturnEmpty200()
        {
            var ids = new List<Guid>();
            chefFinder.FindByIdsAsync(ids).Returns(Array.Empty<Chef>());

            var response = await testObj.GetByBatch(ids);

            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(response.Data);
            Assert.IsEmpty(response.Data);
        }

        [Test]
        public async Task GetByBatch_FoundChefs_ReturnChefs200()
        {
            var chefs = new[] {CreateChef(), CreateChef()};
            chefFinder.FindByIdsAsync(null!).ReturnsForAnyArgs(chefs);

            var response = await testObj.GetByBatch(new List<Guid>());

            Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(2, response.Data!.Count);
        }

        private static Chef CreateChef()
        {
            return Chef.Construct(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    ValidProperties.ChefName)
                .GetModelOrThrow();
        }
    }
}