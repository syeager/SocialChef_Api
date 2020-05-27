using FluentValidation.Validators;
using LittleByte.Asp.Test.Utilities;
using NUnit.Framework;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Domains.Recipes.Validators
{
    public class StepValidatorTest
    {
        private StepValidator testObj;

        [SetUp]
        public void SetUp()
        {
            testObj = new StepValidator();
        }

        [TestCase(StepValidator.NameLengthMin)]
        [TestCase(StepValidator.NameLengthMax)]
        public void Construct_NameLengthValid_Success(int charCount)
        {
            var instruction = new string('a', charCount);
            var step = new Step(instruction);

            var result = testObj.Validate(step);

            Assert.IsTrue(result.IsValid);
        }

        [TestCase('a', StepValidator.NameLengthMin - 1)]
        [TestCase(' ', StepValidator.NameLengthMin)]
        [TestCase('a', StepValidator.NameLengthMax + 1)]
        public void Construct_NameLengthInvalid_ValidationError(char character, int charCount)
        {
            var instruction = new string(character, charCount);
            var step = new Step(instruction);

            var result = testObj.Validate(step);

            result.AssertFirstError(nameof(Step.Instruction), nameof(LengthValidator));
        }
    }
}