using FluentValidation.Validators;
using LittleByte.Domain.Test.Utilities;
using NUnit.Framework;
using SocialChef.Domain.Recipes;

namespace LittleByte.Domain.Test.Models
{
    public class IngredientTest
    {
        private static readonly Quantity validQuantity = new Quantity(1, "name");

        [Test]
        public void Construct_Valid_Success()
        {
            var results = Ingredient.Construct(ValidModelValues.IngredientName, validQuantity);

            Assert.IsTrue(results.IsSuccess);
        }

        [TestCase(Ingredient.Validator.NameLengthMin)]
        [TestCase(Ingredient.Validator.NameLengthMax)]
        public void Construct_NameLengthValid_Success(int charCount)
        {
            var name = new string('a', charCount);

            var result = Ingredient.Construct(name, validQuantity);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestCase('a', Ingredient.Validator.NameLengthMin - 1)]
        [TestCase(' ', Ingredient.Validator.NameLengthMin)]
        [TestCase('a', Ingredient.Validator.NameLengthMax + 1)]
        public void Construct_NameLengthInvalid_ValidationError(char character, int charCount)
        {
            var name = new string(character, charCount);

            var result = Ingredient.Construct(name, validQuantity);

            result.AssertFirstError(nameof(Ingredient.Name), nameof(LengthValidator));
        }
    }
}