using FluentValidation.Validators;
using LittleByte.Asp.Test.Utilities;
using NUnit.Framework;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Domains.Recipes.Validators
{
    public class QuantityValidatorTest
    {
        private const decimal ValidAmount = 1m;
        private const string ValidMeasurement = "C";

        private QuantityValidator testObj;

        [SetUp]
        public void SetUp()
        {
            testObj = new QuantityValidator();
        }

        [Test]
        public void Construct_Valid_Success()
        {
            var quantity = new Quantity(ValidAmount, ValidMeasurement);

            var result = testObj.Validate(quantity);

            Assert.IsTrue(result.IsValid);
        }

        [TestCase(QuantityValidator.AmountMinExclusive + 0.001)]
        [TestCase(QuantityValidator.AmountMax - 0.001)]
        public void Construct_AmountValid_Success(decimal amount)
        {
            var quantity = new Quantity(amount, ValidMeasurement);

            var result = testObj.Validate(quantity);

            Assert.IsTrue(result.IsValid);
        }

        [TestCase(QuantityValidator.AmountMinExclusive)]
        [TestCase(QuantityValidator.AmountMax)]
        public void Construct_AmountInvalid_ValidationError(decimal amount)
        {
            var quantity = new Quantity(amount, ValidMeasurement);

            var result = testObj.Validate(quantity);

            result.AssertFirstError(nameof(Quantity.Amount), nameof(ExclusiveBetweenValidator));
        }

        [TestCase(QuantityValidator.MeasurementLengthMin)]
        [TestCase(QuantityValidator.MeasurementLengthMax)]
        public void Construct_MeasurementValid_Success(int charCount)
        {
            var measurement = new string('a', charCount);
            var quantity = new Quantity(ValidAmount, measurement);

            var result = testObj.Validate(quantity);

            Assert.IsTrue(result.IsValid);
        }

        [TestCase(QuantityValidator.MeasurementLengthMin - 1)]
        [TestCase(QuantityValidator.MeasurementLengthMax + 1)]
        public void Construct_MeasurementInvalid_ValidationError(int charCount)
        {
            var measurement = new string('a', charCount);
            var quantity = new Quantity(ValidAmount, measurement);

            var result = testObj.Validate(quantity);

            result.AssertFirstError(nameof(Quantity.Measurement), nameof(LengthValidator));
        }
    }
}