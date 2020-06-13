using SocialChef.Domain.Document;

namespace SocialChef.Domain.Recipes
{
    public class Quantity
    {
        public decimal Amount { get; }
        public string Measurement { get; }

        public Quantity(decimal amount, string measurement)
        {
            Amount = amount;
            Measurement = measurement.Trim();
        }

        public static implicit operator QuantityDao(Quantity quantity)
        {
            return new QuantityDao(quantity.Amount, quantity.Measurement);
        }

        public static implicit operator Quantity(QuantityDao quantityDao)
        {
            return new Quantity(quantityDao.Amount, quantityDao.Measurement);
        }
    }
}