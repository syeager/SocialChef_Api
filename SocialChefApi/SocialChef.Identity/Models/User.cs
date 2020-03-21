using System;
using Microsoft.AspNetCore.Identity;

namespace SocialChef.Identity.Models
{
    public sealed class User : IdentityUser<Guid>
    {
        public User() {}

        public User(string email)
        {
            UserName = email;
            Email = email;
        }
    }
}