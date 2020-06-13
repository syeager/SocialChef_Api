using SocialChef.Domain.Recipes;

namespace SocialChef.Application.Dtos.Recipes
{
    public class QuantityDto
    {
        public decimal Amount { get; set; }
        public string Measurement { get; set; }

        public QuantityDto()
        {
            Measurement = null!;
        }

        public QuantityDto(decimal amount, string measurement)
        {
            Amount = amount;
            Measurement = measurement;
        }

        public static implicit operator QuantityDto(Quantity quantity)
        {
            return new QuantityDto(
                quantity.Amount,
                quantity.Measurement);
        }

        public static implicit operator Quantity(QuantityDto quantityDto)
        {
            return new Quantity(
                quantityDto.Amount,
                quantityDto.Measurement);
        }
    }
}