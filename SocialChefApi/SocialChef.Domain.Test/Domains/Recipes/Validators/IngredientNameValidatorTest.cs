using FluentValidation.Validators;
using NUnit.Framework;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Domains.Recipes.Validators
{
    public class IngredientNameValidatorTest
    {
        [TestCase(IngredientNameValidator.LengthMin)]
        [TestCase(IngredientNameValidator.LengthMax)]
        public void Validate_NameLengthValid_Success(int charCount)
        {
            var name = new string('a', charCount);
            var sectionName = new IngredientName(name);

            var results = new IngredientNameValidator().Validate(sectionName);

            Assert.IsTrue(results.IsValid);
        }

        [TestCase('a', IngredientNameValidator.LengthMin - 1)]
        [TestCase(' ', IngredientNameValidator.LengthMin)]
        [TestCase('a', IngredientNameValidator.LengthMax + 1)]
        public void Validate_NameLengthInvalid_ValidationError(char character, int count)
        {
            var name = new string(character, count);
            var sectionName = new IngredientName(name);

            var results = new IngredientNameValidator().Validate(sectionName);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(nameof(IngredientName.Value), results.Errors[0].PropertyName);
            Assert.AreEqual(nameof(LengthValidator), results.Errors[0].ErrorCode);
        }
    }
}