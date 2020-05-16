using System;
using System.Threading.Tasks;
using LittleByte.Asp.Test.Database;
using LittleByte.Domain.Test.Utilities;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SocialChef.Domain.Relational;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Chefs
{
    // TODO: Identity server register fail - pass failure on
    public class ChefCreatorTest
    {
        private ChefCreator testObj;
        private IIdentityService identityService;
        private SqlDbContext sqlContext;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DbContextUtility.CreateInMemory(ref sqlContext);
        }

        [SetUp]
        public void SetUp()
        {
            sqlContext.EnsureRecreated();

            identityService = Substitute.For<IIdentityService>();

            testObj = new ChefCreator(identityService, sqlContext);
        }

        [Test]
        public void Create_RegistrationFailure_NoChefCreated()
        {
            var request = new CreateChefRequest();
            identityService.RegisterAsync("", "", "").ThrowsForAnyArgs<Exception>();

            Assert.ThrowsAsync<Exception>(() => testObj.CreateAsync(request));

            Assert.IsFalse(sqlContext.Chefs.AnyAsync().Result);
        }

        [Test]
        public async Task Create_Valid_ChefCreated()
        {
            var request = new CreateChefRequest(
                ValidModelValues.ChefName,
                ValidModelValues.Email,
                ValidModelValues.Password,
                ValidModelValues.Password
            );

            var userId = new User.Guid(Guid.NewGuid());
            identityService.RegisterAsync(
                request.Email,
                request.Password,
                request.PasswordConfirm
            ).Returns(new User(userId));

            var user = await testObj.CreateAsync(request);

            Assert.AreEqual(userId.Value, user.UserId.Value);
        }
    }
}