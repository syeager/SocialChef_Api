using SocialChef.Domain.Identity;
using SocialChef.Domain.Relational;

namespace SocialChef.Domain.Chefs
{
    public class Chef
    {
        public DomainGuid<Chef> ID { get; }
        public DomainGuid<User> UserId { get; }
        public ChefName Name { get; }

        private Chef(DomainGuid<Chef> id, DomainGuid<User> userId, ChefName name)
        {
            ID = id;
            UserId = userId;
            Name = name;
        }

        public static ModelConstructResult<Chef> Construct(DomainGuid<Chef> id, DomainGuid<User> userId, string name)
        {
            var chef = new Chef(id.GetNewIfEmpty().Value, userId, new ChefName(name));

            var validation = new ChefValidator().Validate(chef);

            return ModelConstructResult<Chef>.Construct(chef, validation);
        }

        public static implicit operator ChefDao(Chef chef)
        {
            return new ChefDao(chef.UserId.Value, chef.Name);
        }

        public static implicit operator Chef(ChefDao chefDao)
        {
            return Construct(
                    chefDao.ID,
                    chefDao.UserID,
                    chefDao.Name)
                .GetModelOrThrow();
        }
    }
}