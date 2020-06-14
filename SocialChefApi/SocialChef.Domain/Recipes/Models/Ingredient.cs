using SocialChef.Domain.Document;

namespace SocialChef.Domain.Recipes
{
    public class Ingredient
    {
        public IngredientName Name { get; }
        public Quantity Quantity { get; }

        public Ingredient(IngredientName name, Quantity quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public static implicit operator Ingredient(IngredientDao ingredientDao)
        {
            return new Ingredient(
                new IngredientName(ingredientDao.Name),
                new Quantity(ingredientDao.Amount, ingredientDao.Measurement));
        }

        public static implicit operator IngredientDao(Ingredient ingredient)
        {
            return new IngredientDao(
                ingredient.Name,
                ingredient.Quantity.Amount,
                ingredient.Quantity.Measurement);
        }
    }
}