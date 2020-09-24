using System;
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
        private readonly IAccountService accountService;
        private readonly SqlDbContext sqlContext;

        public ChefCreator(IAccountService accountService, SqlDbContext sqlContext)
        {
            this.accountService = accountService;
            this.sqlContext = sqlContext;
        }

        public Task<Chef> CreateAsync(string name, string email, string password, string passwordConfirm)
        {
            throw new NotImplementedException();
            //var userDto = await accountService.RegisterAsync(email, password, passwordConfirm);

            //var chef = Chef.Construct(null, userDto.Id, name).GetModelOrThrow();
            //ChefDao chefDao = chef;
            //await sqlContext.AddAndSaveAsync(chefDao);

            //return chefDao;
        }
    }
}