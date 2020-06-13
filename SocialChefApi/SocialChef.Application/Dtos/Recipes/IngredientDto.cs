using SocialChef.Domain.Recipes;

namespace SocialChef.Application.Dtos.Recipes
{
    public class IngredientDto
    {
        public string Name { get; set; }
        public QuantityDto Quantity { get; set; }

        public IngredientDto()
        {
            Name = null!;
            Quantity = null!;
        }

        public IngredientDto(string name, QuantityDto quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public static implicit operator IngredientDto(Ingredient ingredient)
        {
            return new IngredientDto(
                ingredient.Name,
                ingredient.Quantity);
        }

        public static implicit operator Ingredient(IngredientDto ingredientDto)
        {
            return new Ingredient(
                new IngredientName(ingredientDto.Name), 
                ingredientDto.Quantity);
        }
    }
}