using System;
using System.Threading.Tasks;
using LittleByte.Asp.Test.Database;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Identity;
using SocialChef.Domain.Relational;
using SocialChef.Domain.Test.Utilities;

namespace SocialChef.Domain.Test.Domains.Chefs.Services
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
            identityService.RegisterAsync("", "", "").ThrowsForAnyArgs<Exception>();

            Assert.ThrowsAsync<Exception>(() => testObj.CreateAsync("", "", "", ""));

            Assert.IsFalse(sqlContext.Chefs.AnyAsync().Result);
        }

        [Test]
        public async Task Create_Valid_ChefCreated()
        {
            var userId = new DomainGuid<User>(Guid.NewGuid());
            identityService.RegisterAsync(
                ValidProperties.Email,
                ValidProperties.Password,
                ValidProperties.Password
            ).Returns(new User(userId));

            var user = await testObj.CreateAsync(
                ValidProperties.ChefName,
                ValidProperties.Email,
                ValidProperties.Password,
                ValidProperties.Password);

            Assert.AreEqual(userId.Value, user.UserId.Value);
        }
    }
}