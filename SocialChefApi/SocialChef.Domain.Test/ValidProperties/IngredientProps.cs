using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Utilities
{
    public partial class Valid
    {
        public static class IngredientProps
        {
            public static readonly IngredientName Name = new IngredientName(new string('a', QuantityValidator.MeasurementLengthMin));
            public static readonly Quantity Quality = new Quantity(1, "C");
            public static readonly Ingredient Ingredient = new Ingredient(Name, Quality);
        }
    }
}