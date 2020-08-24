using System.Net;
using System.Threading.Tasks;
using LittleByte.Asp.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialChef.Application.Dtos;
using SocialChef.Domain.Chefs;
using Controller = LittleByte.Asp.Application.Controller;

namespace SocialChef.Application.Controllers
{
    public class UserController : Controller
    {
        private readonly IChefCreator chefCreator;

        public UserController(IChefCreator chefCreator)
        {
            this.chefCreator = chefCreator;
        }

        [AllowAnonymous]
        [HttpPost]
        [ResponseType(HttpStatusCode.Created, typeof(ChefDto))]
        [ResponseType(HttpStatusCode.BadRequest)]
        public async Task<ApiResult<ChefDto>> Register(CreateChefDto dto)
        {
            var chef = await chefCreator.CreateAsync(dto.Name, dto.Email, dto.Password, dto.PasswordConfirm);
            return new CreatedResult<ChefDto>(chef);
        }
    }
}