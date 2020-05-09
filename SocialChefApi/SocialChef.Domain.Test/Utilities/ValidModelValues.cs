using SocialChef.Domain.Recipes;

namespace LittleByte.Domain.Test.Utilities
{
    public static class ValidModelValues
    {
        public const string Email = "unit@test.com";
        public const string Password = "Abc123$";

        public static readonly string ChefName = new string('a', Chef.Validator.NameLengthMin);
        public static readonly string RecipeName = new string('a', Recipe.Validator.NameLengthMin);
        public static readonly string SectionName = new string('a', Section.Validator.NameLengthMin);
        public static readonly string StepName = new string('a', Step.Validator.NameLengthMin);
        public static readonly string IngredientName = new string('a', Ingredient.Validator.NameLengthMin);
    }
}