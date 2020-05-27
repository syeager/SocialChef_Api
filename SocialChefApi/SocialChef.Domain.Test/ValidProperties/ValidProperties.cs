using SocialChef.Domain.Chefs;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Utilities
{
    public static partial class ValidProperties
    {
        public const string Email = "unit@test.com";
        public const string Password = "Abc123$";

        public static readonly ChefName ChefName = new ChefName(new string('a', ChefNameValidator.NameLengthMin));
        public static readonly RecipeName RecipeName = new RecipeName(new string('a', RecipeNameValidator.LengthMin));
        public static readonly SectionName SectionName = new SectionName(new string('a', SectionNameValidator.LengthMin));
        public static readonly string StepName = new string('a', StepValidator.NameLengthMin);
        public static readonly string IngredientName = new string('a', IngredientValidator.NameLengthMin);
    }
}