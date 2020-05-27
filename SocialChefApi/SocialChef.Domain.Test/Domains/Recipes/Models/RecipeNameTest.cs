using FluentValidation.Validators;
using NUnit.Framework;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Domains.Recipes.Models
{
    public class RecipeNameTest
    {
        [TestCase(RecipeNameValidator.LengthMin)]
        [TestCase(RecipeNameValidator.LengthMax)]
        public void Construct_NameLengthValid_Success(int charCount)
        {
            var name = new string('a', charCount);

            var recipeName = new RecipeName(name);
            var results = new RecipeNameValidator().Validate(recipeName);

            Assert.IsTrue(results.IsValid);
        }

        [TestCase('a', RecipeNameValidator.LengthMin - 1)]
        [TestCase(' ', RecipeNameValidator.LengthMin)]
        [TestCase('a', RecipeNameValidator.LengthMax + 1)]
        public void Construct_NameLengthInvalid_ValidationError(char character, int count)
        {
            var name = new string(character, count);

            var recipeName = new RecipeName(name);
            var results = new RecipeNameValidator().Validate(recipeName);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(nameof(RecipeName.Value), results.Errors[0].PropertyName);
            Assert.AreEqual(nameof(LengthValidator), results.Errors[0].ErrorCode);
        }
    }
}