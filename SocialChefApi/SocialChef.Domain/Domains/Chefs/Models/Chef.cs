using FluentValidation;
using LittleByte.Domain;
using SocialChef.Domain.Relational;

namespace SocialChef.Domain.Recipes
{
    public class Chef
    {
        public class Validator : AbstractValidator<Chef>
        {
            public const int NameLengthMin = 5;
            public const int NameLengthMax = 50;

            public static readonly Validator Instance = new Validator();

            private Validator()
            {
                RuleFor(c => c.UserId).NotEmpty();
                RuleFor(c => c.Name).Length(NameLengthMin, NameLengthMax);
            }
        }

        public readonly struct Guid
        {
            public System.Guid Value { get; }

            public Guid(System.Guid value)
            {
                Value = value;
            }
        }

        public Guid ID { get; }
        public User.Guid UserId { get; }
        public string Name { get; }

        private Chef(Guid id, User.Guid userId, string name)
        {
            ID = id;
            UserId = userId;
            Name = name.Trim();
        }

        public static ModelConstructResult<Chef> Construct(Guid? id, User.Guid userId, string name)
        {
            id ??= new Guid(System.Guid.NewGuid());
            var chef = new Chef(id.Value, userId, name);

            var validation = Validator.Instance.Validate(chef);

            return ModelConstructResult<Chef>.Construct(chef, validation);
        }

        public static implicit operator ChefDao(Chef chef)
        {
            return new ChefDao(chef.UserId.Value, chef.Name);
        }

        public static implicit operator Chef(ChefDao chefDao)
        {
            return Construct(
                    new Guid(chefDao.ID),
                    new User.Guid(chefDao.UserID),
                    chefDao.Name)
                .GetModelOrThrow();
        }
    }
}