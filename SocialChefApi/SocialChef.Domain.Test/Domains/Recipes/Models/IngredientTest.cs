using NUnit.Framework;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Domains.Recipes.Models
{
    public class IngredientTest
    {
        private static readonly Quantity validQuantity = new Quantity(1, "name");

        [Test]
        public void Ctor_InstructionWhitespace_Trimmed()
        {
            var step = new Ingredient(" a ", validQuantity);

            Assert.AreEqual("a", step.Name);
        }
    }
}