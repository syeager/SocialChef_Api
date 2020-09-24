using System;
using System.Net;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SocialChef.Application.Controllers;
using SocialChef.Application.Dtos;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Identity;
using SocialChef.Domain.Test.Utilities;

namespace SocialChef.Application.Test
{
    public class UserControllerTest
    {
        private AccountController testObj;
        private IChefCreator chefCreator;
        private IAccountService accountService;

        [SetUp]
        public void SetUp()
        {
            chefCreator = Substitute.For<IChefCreator>();
            accountService = Substitute.For<IAccountService>();

            testObj = new AccountController(chefCreator, accountService);
        }

        // TODO: Enable.
        //[Test]
        //public async Task Register_CreatedChef_ReturnChef201()
        //{
        //    var dto = new CreateChefDto();
        //    var chef = Chef.Construct(Guid.NewGuid(), Guid.NewGuid(), ValidProperties.ChefName).GetModelOrThrow();
        //    chefCreator.CreateAsync("", "", "", "").ReturnsForAnyArgs(chef);

        //    var response = await testObj.Register(dto);

        //    Assert.AreEqual((int)HttpStatusCode.Created, response.StatusCode);
        //    Assert.AreEqual(chef.ID.Value, response.Data!.ID);
        //}
    }
}