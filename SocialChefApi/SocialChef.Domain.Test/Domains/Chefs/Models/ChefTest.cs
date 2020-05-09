using System;
using FluentValidation.Validators;
using LittleByte.Domain.Test.Utilities;
using NUnit.Framework;
using SocialChef.Domain.Recipes;

namespace LittleByte.Domain.Test.Models
{
    public class ChefTest
    {
        private static readonly Chef.Guid? validID = new Chef.Guid(Guid.NewGuid());
        private static readonly User.Guid validChefId = new User.Guid(Guid.NewGuid());

        [Test]
        public void Construct_Valid_Success()
        {
            var results = Chef.Construct(validID, validChefId, ValidModelValues.ChefName);

            Assert.IsTrue(results.IsSuccess);
        }

        [Test]
        public void Construct_IDNull_NewID()
        {
            var results = Chef.Construct(null, validChefId, ValidModelValues.ChefName);

            Assert.IsTrue(results.IsSuccess);
            Assert.AreNotEqual(Guid.Empty, results.Model!.ID);
        }

        [Test]
        public void Construct_EmptyUser_ValidationError()
        {
            var userId = new User.Guid(Guid.Empty);
            var results = Chef.Construct(null, userId, ValidModelValues.ChefName);

            results.AssertFirstError(nameof(Chef.UserId), nameof(NotEmptyValidator));
        }

        [TestCase(Chef.Validator.NameLengthMin)]
        [TestCase(Chef.Validator.NameLengthMax)]
        public void Construct_NameLengthValid_Success(int charCount)
        {
            var name = new string('a', charCount);

            var results = Chef.Construct(validID, validChefId, name);

            Assert.IsTrue(results.IsSuccess);
        }

        [TestCase('a', Chef.Validator.NameLengthMin - 1)]
        [TestCase(' ', Chef.Validator.NameLengthMin)]
        [TestCase('a', Chef.Validator.NameLengthMax + 1)]
        public void Construct_NameLengthInvalid_ValidationError(char character, int count)
        {
            var name = new string(character, count);

            var results = Chef.Construct(validID, validChefId, name);

            results.AssertFirstError(nameof(Chef.Name), nameof(LengthValidator));
        }
    }
}