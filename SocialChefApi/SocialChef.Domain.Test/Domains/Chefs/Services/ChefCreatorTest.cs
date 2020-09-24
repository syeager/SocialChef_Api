using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
    public class ChefCreatorTest
    {
        private ChefCreator testObj;
        private IAccountService accountService;
        private SqlDbContext sqlContext;

        [SetUp]
        public void SetUp()
        {
            DbContextFactory.BuildSql(ref sqlContext);

            accountService = Substitute.For<IAccountService>();

            testObj = new ChefCreator(accountService, sqlContext);
        }

        [Test]
        public void Create_RegistrationFailure_NoChefCreated()
        {
            accountService.RegisterAsync("", "", "").ThrowsForAnyArgs<Exception>();

            Assert.ThrowsAsync<Exception>(() => testObj.CreateAsync("", "", "", ""));

            Assert.IsFalse(sqlContext.Chefs.AnyAsync().Result);
        }

        [Test]
        public async Task Create_Valid_ChefCreated()
        {
            var userId = new DomainGuid<User>(Guid.NewGuid());
            accountService.RegisterAsync(
                ValidProperties.Email,
                ValidProperties.Password,
                ValidProperties.Password
            ).Returns(IdentityResult.Success);

            var user = await testObj.CreateAsync(
                ValidProperties.ChefName,
                ValidProperties.Email,
                ValidProperties.Password,
                ValidProperties.Password);

            Assert.AreEqual(userId.Value, user.UserId.Value);
        }
    }
}