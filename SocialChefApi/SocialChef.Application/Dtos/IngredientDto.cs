namespace SocialChef.Domain.DTOs
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
    }
}