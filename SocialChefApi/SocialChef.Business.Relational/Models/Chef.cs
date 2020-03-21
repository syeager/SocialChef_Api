using System;
using System.Collections.Generic;
using LittleByte.Asp.Database;

namespace SocialChef.Business.Relational.Models
{
    public class Chef : Entity
    {
        public Guid UserID { get; set; }
        public string Name { get; set; }

        public List<ChefRecipe>? Recipes { get; set; }

        public Chef(Guid userID, string name)
        {
            UserID = userID;
            Name = name;
        }
    }
}