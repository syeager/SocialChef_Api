using System;

namespace SocialChef.Persistence.Models
{
    public class ChefRecipe
    {
        public Guid ChefID { get; set; }
        public Guid RecipeID { get; set; }

        public Chef? Chef { get; set; }
    }
}