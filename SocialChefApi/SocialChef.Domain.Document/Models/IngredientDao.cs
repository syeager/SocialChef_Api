using JetBrains.Annotations;

namespace SocialChef.Domain.Document
{
    public class IngredientDao
    {
        public string Name { get; set; }
        public QuantityDao Quantity { get; set; }

        [UsedImplicitly]
        public IngredientDao()
        {
            Name = null!;
            Quantity = null!;
        }

        public IngredientDao(string name, QuantityDao quantity)
        {
            Name = name;
            Quantity = quantity;
        }
    }
}