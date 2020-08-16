namespace SocialChef.Application.Dtos
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
    }
}