using FluentValidation.Validators;
using NUnit.Framework;
using SocialChef.Domain.Chefs;

namespace SocialChef.Domain.Test.Domains.Chefs.Models
{
    public class ChefNameTest
    {
        [TestCase(ChefNameValidator.NameLengthMin)]
        [TestCase(ChefNameValidator.NameLengthMax)]
        public void Constructor_NameLengthValid_Success(int charCount)
        {
            var name = new string('a', charCount);

            var chefName = new ChefName(name);
            var results = new ChefNameValidator().Validate(chefName);

            Assert.IsTrue(results.IsValid);
        }

        [TestCase('a', ChefNameValidator.NameLengthMin - 1)]
        [TestCase(' ', ChefNameValidator.NameLengthMin)]
        [TestCase('a', ChefNameValidator.NameLengthMax + 1)]
        public void Constructor_NameLengthInvalid_ValidationError(char character, int count)
        {
            var name = new string(character, count);

            var chefName = new ChefName(name);
            var results = new ChefNameValidator().Validate(chefName);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(nameof(ChefName.Value), results.Errors[0].PropertyName);
            Assert.AreEqual(nameof(LengthValidator), results.Errors[0].ErrorCode);
        }
    }
}