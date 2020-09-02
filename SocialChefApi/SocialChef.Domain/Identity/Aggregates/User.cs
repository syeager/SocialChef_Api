using System;
using SocialChef.Domain.Relational;

namespace SocialChef.Domain.Identity
{
    // TODO: Combine chef and user.
    public class User
    {
        public DomainGuid<User> Id { get; }

        // TODO: Don't allow Empty.
        public User(DomainGuid<User> id)
        {
            Id = id;
        }

        // TODO: Add validation.
        public static implicit operator User(UserDao chefDao)
        {
            return new User(new DomainGuid<User>(new Guid(chefDao.Id)));
        }
    }
}