namespace SocialChef.Domain.Recipes
{
    public class Ingredient
    {
        public IngredientName Name { get; }
        public Quantity Quantity { get; }

        internal Ingredient(IngredientName name, Quantity quantity)
        {
            Name = name;
            Quantity = quantity;
        }
    }
}