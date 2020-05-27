using FluentValidation.Validators;
using LittleByte.Asp.Test.Utilities;
using NUnit.Framework;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Domains.Recipes.Validators
{
    public class IngredientValidatorTest
    {
        private static readonly Quantity validQuantity = new Quantity(1, "name");

        private IngredientValidator testObj;

        [SetUp]
        public void SetUp()
        {
            testObj = new IngredientValidator();
        }

        [TestCase(IngredientValidator.NameLengthMin)]
        [TestCase(IngredientValidator.NameLengthMax)]
        public void Construct_NameLengthValid_Success(int charCount)
        {
            var name = new string('a', charCount);
            var ingredient = new Ingredient(name, validQuantity);

            var result = testObj.Validate(ingredient);

            Assert.IsTrue(result.IsValid);
        }

        [TestCase('a', IngredientValidator.NameLengthMin - 1)]
        [TestCase(' ', IngredientValidator.NameLengthMin)]
        [TestCase('a', IngredientValidator.NameLengthMax + 1)]
        public void Construct_NameLengthInvalid_ValidationError(char character, int charCount)
        {
            var name = new string(character, charCount);
            var ingredient = new Ingredient(name, validQuantity);

            var result = testObj.Validate(ingredient);

            result.AssertFirstError(nameof(Ingredient.Name), nameof(LengthValidator));
        }
    }
}