using System.Threading.Tasks;
using LittleByte.Asp.Database;
using SocialChef.Domain.Relational;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Chefs
{
    public interface IChefCreator
    {
        Task<Chef> CreateAsync(CreateChefRequest request);
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

        public async Task<Chef> CreateAsync(CreateChefRequest request)
        {
            var userDto = await identityService.RegisterAsync(request.Email, request.Password, request.PasswordConfirm);

            var chef = Chef.Construct(null, userDto.Id, request.Name).GetModelOrThrow();
            ChefDao chefDao = chef;
            await sqlContext.AddAndSaveAsync(chefDao);

            return chefDao;
        }
    }
}