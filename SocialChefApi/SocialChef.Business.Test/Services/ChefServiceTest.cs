using System;
using System.Threading.Tasks;
using LittleByte.Asp.Exceptions;
using LittleByte.Asp.Test.Database;
using NSubstitute;
using NUnit.Framework;
using SocialChef.Business.Relational.Contexts;
using SocialChef.Business.Relational.Models;
using SocialChef.Business.Requests;
using SocialChef.Business.Services;
using SocialChef.Identity.Transport;

namespace SocialChef.Business.Test
{
    public sealed class ChefServiceTest
    {
        private ChefService testObj;
        private SqlDbContext sqlContext;
        private IIdentityService identityService;

        private Guid userID;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DbContextUtility.CreateInMemory(ref sqlContext, "sql");
        }

        [SetUp]
        public void SetUp()
        {
            sqlContext.EnsureRecreated();
            identityService = Substitute.For<IIdentityService>();

            testObj = new ChefService(sqlContext, identityService);

            userID = Guid.NewGuid();
        }

        [Test]
        public async Task Create_HasUserNoChef_NewChef()
        {
            var request = new CreateChefRequest("bob", "b@b.com", "abc", "abc");
            identityService.RegisterAsync(request.Email, request.Password, request.PasswordConfirm).Returns(new UserDto(userID));

            var chef = await testObj.CreateAsync(request);

            Assert.NotNull(chef);
        }

        [Test]
        public void Get_NoChef_ThrowNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => testObj.GetChefAsync(Guid.NewGuid()));
        }

        [Test]
        public async Task Get_HasChef_ReturnChef()
        {
            var chef = sqlContext.AddAndSave(new Chef(userID, "bob"));

            var foundChef = await testObj.GetChefAsync(chef.ID);

            Assert.AreEqual(chef.ID, foundChef.ID);
        }

        [Test]
        public void GetChefByUserID_EmptyGuid_ThrowNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => testObj.GetChefByUserIDAsync(Guid.Empty));
        }

        [Test]
        public void GetChefByUserID_NoChef_ThrowNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => testObj.GetChefByUserIDAsync(userID));
        }

        [Test]
        public async Task GetChefByUserID_HasUser_ReturnUser()
        {
            var chef = sqlContext.AddAndSave(new Chef(userID, "bob"));

            var chefDto = await testObj.GetChefByUserIDAsync(chef.UserID);

            Assert.AreEqual(chef.ID, chefDto.ID);
        }
    }
}