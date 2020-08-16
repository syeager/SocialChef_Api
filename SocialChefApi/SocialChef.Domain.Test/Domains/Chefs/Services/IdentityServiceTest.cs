using LittleByte.Asp.Exceptions;
using LittleByte.Asp.Test.Fakes;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;
using SocialChef.Domain.Identity;
using SocialChef.Domain.Relational;

namespace SocialChef.Domain.Test.Domains.Chefs.Services
{
    internal class IdentityServiceTest
    {
        private static class Valid
        {
            public const string Password = "abc";
            public const string Email = "abc";
        }

        private IdentityService testObj;
        private FakeUserManager<UserDao> userManager;

        [SetUp]
        public void Setup()
        {
            userManager = Substitute.For<FakeUserManager<UserDao>>();

            testObj = new IdentityService(userManager);
        }

        [Test]
        public void Register_CreateFail_ThrowBadRequest()
        {
            Assert.ThrowsAsync<BadRequestException>(() => testObj.RegisterAsync(Valid.Email, "abc", "def"));
        }

        [Test]
        public void Register_NoEmail_ThrowBadRequest()
        {
            Assert.ThrowsAsync<BadRequestException>(() => testObj.RegisterAsync("not-an-email", Valid.Password, Valid.Password));
        }

        [Test]
        public void Register_CreateFailure_ThrowServerError()
        {
            userManager.CreateAsync(Arg.Any<UserDao>(), Valid.Password).Returns(IdentityResult.Failed());

            Assert.ThrowsAsync<BadRequestException>(() => testObj.RegisterAsync(Valid.Email, Valid.Password, Valid.Password));
        }
    }
}