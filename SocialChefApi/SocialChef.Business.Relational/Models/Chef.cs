using System;
using System.Collections.Generic;
using LittleByte.Asp.Database;

namespace SocialChef.Business.Relational.Models
{
    public class Chef: Entity
    {
        public Guid UserID { get; set; }

        public Data.User.Models.User? User { get; set; }
        public List<ChefRecipe>? Recipes { get; set; }
    }
}