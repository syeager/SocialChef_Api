using System.Threading.Tasks;
using LittleByte.Asp.Database;
using SocialChef.Domain.Identity;
using SocialChef.Domain.Relational;

namespace SocialChef.Domain.Chefs
{
    public interface IChefCreator
    {
        Task<Chef> CreateAsync(string name, string email, string password, string passwordConfirm);
    }

    public class ChefCreator : IChefCreator
    {
        private readonly IIdentityService identityService;
        private readonly SqlDbContext sqlContext;

        public ChefCreator(IIdentityService identityService, SqlDbContext sqlContext)
        {
            this.identityService = identityService;
            this.sqlContext = sqlContext;
        }

        public async Task<Chef> CreateAsync(string name, string email, string password, string passwordConfirm)
        {
            var userDto = await identityService.RegisterAsync(email, password, passwordConfirm);

            var chef = Chef.Construct(null, userDto.Id, name).GetModelOrThrow();
            ChefDao chefDao = chef;
            await sqlContext.AddAndSaveAsync(chefDao);

            return chefDao;
        }
    }
}