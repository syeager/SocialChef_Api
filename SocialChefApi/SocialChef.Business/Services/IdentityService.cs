using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LittleByte.Asp.Exceptions;
using LittleByte.Asp.Validation;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SocialChef.Business.ConfigOptions;
using SocialChef.Identity.Transport;

namespace SocialChef.Business.Services
{
    public interface IIdentityService
    {
        Task<UserDto> RegisterAsync(string email, string password, string passwordConfirm);
    }

    internal class IdentityService : IIdentityService
    {
        private readonly HttpClient httpClient;
        private readonly string identityServerUrl;

        public IdentityService(HttpClient httpClient, IOptions<IdentityOptions> identityOptions)
        {
            this.httpClient = httpClient;
            identityServerUrl = identityOptions.Value.Address;
        }

        public async Task<UserDto> RegisterAsync(string email, string password, string passwordConfirm)
        {
            if(password != passwordConfirm)
            {
                throw new BadRequestException("Passwords do not match.");
            }

            if(!email.IsEmail())
            {
                throw new BadRequestException($"'{email}' is not a valid email.");
            }

            var request = new RegisterRequest(email, password);
            var requestJson = JsonConvert.SerializeObject(request);
            var stringContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(new Uri($"{identityServerUrl}/account/register"), stringContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            if(response.IsSuccessStatusCode)
            {
                var userDto = JsonConvert.DeserializeObject<UserDto>(responseBody);
                return userDto;
            }

            var exception = response.StatusCode switch
            {
                HttpStatusCode.BadRequest => new BadRequestException(responseBody),
                _ => new HttpException(HttpStatusCode.InternalServerError, responseBody)
            };
            throw exception;
        }
    }
}