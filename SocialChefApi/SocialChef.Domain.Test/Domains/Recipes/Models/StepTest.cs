using FluentValidation.Validators;
using LittleByte.Domain.Test.Utilities;
using NUnit.Framework;
using SocialChef.Domain.Recipes;

namespace LittleByte.Domain.Test.Models
{
    public class StepTest
    {
        [Test]
        public void Construct_Valid_Success()
        {
            var result = Step.Construct(ValidModelValues.StepName);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestCase(Step.Validator.NameLengthMin)]
        [TestCase(Step.Validator.NameLengthMax)]
        public void Construct_NameLengthValid_Success(int charCount)
        {
            var name = new string('a', charCount);

            var result = Step.Construct(name);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestCase('a', Step.Validator.NameLengthMin - 1)]
        [TestCase(' ', Step.Validator.NameLengthMin)]
        [TestCase('a', Step.Validator.NameLengthMax + 1)]
        public void Construct_NameLengthInvalid_ValidationError(char character, int charCount)
        {
            var name = new string(character, charCount);

            var result = Step.Construct(name);

            result.AssertFirstError(nameof(Step.Instruction), nameof(LengthValidator));
        }
    }
}