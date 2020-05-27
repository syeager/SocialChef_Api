namespace SocialChef.Domain.Recipes
{
    public class Quantity
    {
        public decimal Amount { get; }
        public string Measurement { get; }

        internal Quantity(decimal amount, string measurement)
        {
            Amount = amount;
            Measurement = measurement.Trim();
        }
    }
}