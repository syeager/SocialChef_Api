using System;
using JetBrains.Annotations;
using LittleByte.Asp.Business;

namespace SocialChef.Business.DTOs
{
    public class ChefDto : Dto
    {
        public string Name { get; set; }

        [UsedImplicitly]
        public ChefDto()
        {
            Name = null!;
        }

        public ChefDto(Guid id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}