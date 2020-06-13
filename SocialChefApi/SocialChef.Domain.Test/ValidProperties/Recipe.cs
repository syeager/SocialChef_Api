using System.Collections.Generic;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Utilities
{
    public static partial class Valid
    {
        public static class Recipe
        {
            public static readonly RecipeName Name = new RecipeName(new string('a', RecipeNameValidator.LengthMin));
            public static readonly Ingredient[] Ingredient = { Ingredients.Ingredient };
        }
    }
}