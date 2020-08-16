using System;
using System.Threading.Tasks;
using LittleByte.Asp.Exceptions;
using LittleByte.Asp.Validation;
using Microsoft.AspNetCore.Identity;
using SocialChef.Domain.Relational;

namespace SocialChef.Domain.Identity
{
    public interface IIdentityService
    {
        Task<User> RegisterAsync(string email, string password, string passwordConfirm);
    }

    internal class IdentityService : IIdentityService
    {
        private readonly UserManager<UserDao> userManager;

        public IdentityService(UserManager<UserDao> userManager)
        {
            this.userManager = userManager;
        }

        // TODO: Send confirmation email.
        public async Task<User> RegisterAsync(string email, string password, string passwordConfirm)
        {
            if(password != passwordConfirm)
            {
                throw new BadRequestException("Passwords do not match.");
            }

            if(!email.IsEmail())
            {
                throw new BadRequestException($"'{email}' is not a valid email.");
            }

            var userDao = new UserDao {Email = email, UserName = email};
            var result = await userManager.CreateAsync(userDao, password);

            if(result.Succeeded)
            {
                var user = new User(new DomainGuid<User>(Guid.Parse(userDao.Id)));
                return user;
            }

            var error = string.Join(", ", result.Errors);
            throw new BadRequestException($"Failed to register: {error}");
        }
    }
}