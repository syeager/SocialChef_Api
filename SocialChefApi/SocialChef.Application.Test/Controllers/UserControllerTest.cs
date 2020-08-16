using System;
using System.Net;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SocialChef.Application.Controllers;
using SocialChef.Application.Dtos.Chefs;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Test.Utilities;

namespace SocialChef.Application.Test
{
    public class UserControllerTest
    {
        private UserController testObj;
        private IChefCreator chefCreator;

        [SetUp]
        public void SetUp()
        {
            chefCreator = Substitute.For<IChefCreator>();

            testObj = new UserController(chefCreator);
        }

        [Test]
        public async Task Register_CreatedChef_ReturnChef201()
        {
            var dto = new CreateChefDto();
            var chef = Chef.Construct(Guid.NewGuid(), Guid.NewGuid(), ValidProperties.ChefName).GetModelOrThrow();
            chefCreator.CreateAsync("", "", "", "").ReturnsForAnyArgs(chef);

            var response = await testObj.Register(dto);

            Assert.AreEqual((int)HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(chef.ID.Value, response.Data!.ID);
        }
    }
}