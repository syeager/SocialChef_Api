using System;
using LittleByte.Asp.Business;
using SocialChef.Domain.Recipes;

namespace SocialChef.Application.Controllers
{
    public sealed class ChefDto : Dto
    {
        public string Name { get; set; }

        public ChefDto(Guid id, string name)
            : base(id)
        {
            Name = name;
        }

        public static implicit operator ChefDto(Chef chef)
        {
            return new ChefDto(chef.ID.Value, chef.Name);
        }
    }
}