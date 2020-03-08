using System;
using System.Collections.Generic;
using LittleByte.Asp.Database;

namespace SocialChef.Persistence.Models
{
    public class Chef: Entity
    {
        public Guid UserID { get; set; }

        public ApplicationUser? User { get; set; }
        public List<ChefRecipe>? Recipes { get; set; }
    }
}