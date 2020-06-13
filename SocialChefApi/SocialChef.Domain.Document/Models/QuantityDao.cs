using JetBrains.Annotations;

namespace SocialChef.Domain.Document
{
    public class QuantityDao
    {
        public decimal Amount { get; set; }
        public string Measurement { get; set; }

        [UsedImplicitly]
        public QuantityDao()
        {
            Measurement = null!;
        }

        public QuantityDao(decimal amount, string measurement)
        {
            Amount = amount;
            Measurement = measurement;
        }
    }
}