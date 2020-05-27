using FluentValidation.Validators;
using NUnit.Framework;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Domains.Recipes.Validators
{
    public class SectionNameValidatorTest
    {
        [TestCase(SectionNameValidator.LengthMin)]
        [TestCase(SectionNameValidator.LengthMax)]
        public void Validate_NameLengthValid_Success(int charCount)
        {
            var name = new string('a', charCount);
            var sectionName = new SectionName(name);

            var results = new SectionNameValidator().Validate(sectionName);

            Assert.IsTrue(results.IsValid);
        }

        [TestCase('a', RecipeNameValidator.LengthMin - 1)]
        [TestCase(' ', RecipeNameValidator.LengthMin)]
        [TestCase('a', RecipeNameValidator.LengthMax + 1)]
        public void Validate_NameLengthInvalid_ValidationError(char character, int count)
        {
            var name = new string(character, count);
            var sectionName = new SectionName(name);

            var results = new SectionNameValidator().Validate(sectionName);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(nameof(SectionName.Value), results.Errors[0].PropertyName);
            Assert.AreEqual(nameof(LengthValidator), results.Errors[0].ErrorCode);
        }
    }
}