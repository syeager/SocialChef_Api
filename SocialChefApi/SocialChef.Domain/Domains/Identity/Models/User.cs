namespace SocialChef.Domain.Recipes
{
    public class User
    {
        public readonly struct Guid
        {
            public System.Guid Value { get; }

            public Guid(System.Guid value)
            {
                Value = value;
            }
        }

        public Guid Id { get; }

        // TODO: Don't allow Empty.
        public User(Guid id)
        {
            Id = id;
        }
    }
}