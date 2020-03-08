using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SocialChef.Data.User.Contexts
{
    public class UserDbContext : IdentityDbContext<Models.User, IdentityRole<Guid>, Guid>
    {

        public UserDbContext(DbContextOptions<UserDbContext> options)
            :base(options)
        {
        }
    }
}