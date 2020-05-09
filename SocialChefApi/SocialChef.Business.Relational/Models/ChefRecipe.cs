using System;

namespace SocialChef.Business.Relational
{
    // TODO: Do I need this?
    public class ChefRecipe
    {
        public Guid ChefID { get; set; }
        public Guid RecipeID { get; set; }

        public ChefDao? Chef { get; set; }

        public ChefRecipe(Guid chefID, Guid recipeID)
        {
            ChefID = chefID;
            RecipeID = recipeID;
        }
    }
}