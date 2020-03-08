using System;
using System.Collections.Generic;
using LittleByte.Asp.Business;

namespace SocialChef.Business.DTOs
{
    public class RecipeDto : Dto
    {
        public string Name { get; set; }
        public ICollection<string> Steps { get; set; }

        public RecipeDto(Guid id, string name, ICollection<string> steps)
        {
            Name = name;
            ID = id;
            Steps = steps;
        }
    }
}