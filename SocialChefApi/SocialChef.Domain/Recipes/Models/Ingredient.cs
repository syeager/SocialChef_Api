namespace SocialChef.Domain.Recipes
{
    public class Ingredient
    {
        // TODO: Make IngriedientName.
        public string Name { get; }
        public Quantity Quantity { get; }

        internal Ingredient(string name, Quantity quantity)
        {
            Quantity = quantity;
            Name = name.Trim();
        }
    }
}