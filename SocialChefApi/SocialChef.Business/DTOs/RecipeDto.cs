using System;
using System.Collections.Generic;
using LittleByte.Asp.Business;

namespace SocialChef.Business.DTOs
{
    public class RecipeDto : Dto
    {
        public string Name { get; }
        public ICollection<string> Steps { get; }
        public ChefDto Chef { get; }

        public RecipeDto(Guid id, string name, ICollection<string> steps, ChefDto chef)
        {
            Name = name;
            ID = id;
            Steps = steps;
            Chef = chef;
        }
    }
}