namespace SocialChef.Domain.Identity
{
    public class User
    {
        public DomainGuid<User> Id { get; }

        // TODO: Don't allow Empty.
        public User(DomainGuid<User> id)
        {
            Id = id;
        }
    }
}