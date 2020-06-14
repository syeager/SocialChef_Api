using JetBrains.Annotations;

namespace SocialChef.Domain.Document
{
    public class IngredientDao
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Measurement { get; set; }

        [UsedImplicitly]
        public IngredientDao()
        {
            Name = null!;
            Measurement = null!;
        }

        public IngredientDao(string name, decimal amount, string measurement)
        {
            Name = name;
            Amount = amount;
            Measurement = measurement;
        }
    }
}