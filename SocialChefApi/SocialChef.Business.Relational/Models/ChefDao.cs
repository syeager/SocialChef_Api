using System;
using System.Collections.Generic;
using LittleByte.Asp.Database;

namespace SocialChef.Business.Relational
{
    public class ChefDao : Entity
    {
        public Guid UserID { get; set; }
        public string Name { get; set; }

        public List<ChefRecipe>? Recipes { get; set; }

        public ChefDao(Guid userID, string name)
        {
            UserID = userID;
            Name = name;
        }
    }
}