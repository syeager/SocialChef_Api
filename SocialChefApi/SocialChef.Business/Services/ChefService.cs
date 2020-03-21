using System;
using System.Threading.Tasks;
using LittleByte.Asp.Database;
using LittleByte.Asp.Exceptions;
using Microsoft.EntityFrameworkCore;
using SocialChef.Business.DTOs;
using SocialChef.Business.Relational.Contexts;
using SocialChef.Business.Relational.Models;
using SocialChef.Business.Requests;
using SocialChef.Identity.Transport;

namespace SocialChef.Business.Services
{
    public interface IChefService
    {
        Task<ChefDto> CreateAsync(CreateChefRequest request);
        Task<ChefDto> GetChefAsync(Guid chefID);
        Task<ChefDto> GetChefByUserIDAsync(Guid userID);
        ChefDto ToDto(Chef entity);
    }

    internal class ChefService : IChefService
    {
        private readonly SqlDbContext sqlDbContext;
        private readonly IIdentityService identityService;

        public ChefService(SqlDbContext sqlDbContext, IIdentityService identityService)
        {
            this.sqlDbContext = sqlDbContext;
            this.identityService = identityService;
        }

        public async Task<ChefDto> CreateAsync(CreateChefRequest request)
        {
            var user = await identityService.RegisterAsync(request.Email, request.Password, request.PasswordConfirm);

            var chef = new Chef(user.ID, request.Name);
            await sqlDbContext.AddAndSaveAsync(chef);

            return ToDto(chef);
        }

        public async Task<ChefDto> GetChefAsync(Guid chefID)
        {
            var chef = await sqlDbContext.Chefs.FindAsync(chefID);
            if(chef == null)
            {
                throw new NotFoundException(typeof(ChefDto), chefID);
            }

            return ToDto(chef);
        }

        public async Task<ChefDto> GetChefByUserIDAsync(Guid userID)
        {
            if(userID == Guid.Empty)
            {
                throw new NotFoundException(typeof(UserDto), userID);
            }

            var chef = await sqlDbContext.Chefs.FirstOrDefaultAsync(c => c.UserID == userID);
            if(chef == null)
            {
                throw new NotFoundException(typeof(UserDto), userID);
            }

            return ToDto(chef);
        }

        public ChefDto ToDto(Chef entity)
        {
            return new ChefDto(entity.ID, entity.Name);
        }
    }
}