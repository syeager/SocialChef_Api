using System.Net;
using LittleByte.Asp.Exceptions;
using LittleByte.Asp.Test.Mocks;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using SocialChef.Business.ConfigOptions;
using SocialChef.Business.Services;
using SocialChef.Identity.Transport;

namespace SocialChef.Identity.Test
{
    public class IdentityServiceTest
    {
        private IdentityService testObj;
        private MockHttpMessageHandler httpMessageHandler;

        [SetUp]
        public void Setup()
        {
            var httpClient = MockHttpMessageHandler.Create(out httpMessageHandler);
            var identityOptions = Options.Create(new IdentityOptions {Address = "http://test"});

            testObj = new IdentityService(httpClient, identityOptions);
        }

        [Test]
        public void Register_PasswordConfirmFail_ThrowBadRequest()
        {
            httpMessageHandler.SetResponse(HttpStatusCode.OK, new UserDto());

            Assert.ThrowsAsync<BadRequestException>(() => testObj.RegisterAsync("a@a.com", "abc", "def"));
        }

        [Test]
        public void Register_NoEmail_ThrowBadRequest()
        {
            httpMessageHandler.SetResponse(HttpStatusCode.OK, new UserDto());

            Assert.ThrowsAsync<BadRequestException>(() => testObj.RegisterAsync("not-an-email", "abc", "abc"));
        }

        [TestCase(HttpStatusCode.InternalServerError)]
        public void Register_IdentityServerFailureCode_ThrowServerError(HttpStatusCode statusCode)
        {
            httpMessageHandler.SetResponse(statusCode, "some error message");

            Assert.ThrowsAsync<HttpException>(() => testObj.RegisterAsync("a@a.com", "abc", "abc"));
        }

        [Test]
        public void Register_IdentityServer400_ThrowBadRequest()
        {
            httpMessageHandler.SetResponse(HttpStatusCode.BadRequest, "Duplicate user");

            Assert.ThrowsAsync<BadRequestException>(() => testObj.RegisterAsync("a@a.com", "abc", "abc"));
        }
    }
}