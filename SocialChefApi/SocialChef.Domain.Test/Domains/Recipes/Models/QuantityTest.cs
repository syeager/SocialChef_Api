using FluentValidation.Validators;
using LittleByte.Domain.Test.Utilities;
using NUnit.Framework;
using SocialChef.Domain.Recipes;

namespace LittleByte.Domain.Test.Models
{
    public class QuantityTest
    {
        private const decimal ValidAmount = 1m;
        private const string ValidMeasurement = "C";

        [Test]
        public void Construct_Valid_Success()
        {
            var result = Quantity.Construct(ValidAmount, ValidMeasurement);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestCase(Quantity.Validator.AmountMinExclusive + 0.001)]
        [TestCase(Quantity.Validator.AmountMax - 0.001)]
        public void Construct_AmountValid_Success(decimal amount)
        {
            var result = Quantity.Construct(amount, ValidMeasurement);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestCase(Quantity.Validator.AmountMinExclusive)]
        [TestCase(Quantity.Validator.AmountMax)]
        public void Construct_AmountInvalid_ValidationError(decimal amount)
        {
            var result = Quantity.Construct(amount, ValidMeasurement);

            result.AssertFirstError(nameof(Quantity.Amount), nameof(ExclusiveBetweenValidator));
        }

        [TestCase(Quantity.Validator.MeasurementLengthMin)]
        [TestCase(Quantity.Validator.MeasurementLengthMax)]
        public void Construct_MeasurementValid_Success(int charCount)
        {
            var measurement = new string('a', charCount);

            var result = Quantity.Construct(ValidAmount, measurement);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestCase(Quantity.Validator.MeasurementLengthMin - 1)]
        [TestCase(Quantity.Validator.MeasurementLengthMax + 1)]
        public void Construct_MeasurementInvalid_ValidationError(int charCount)
        {
            var measurement = new string('a', charCount);

            var result = Quantity.Construct(ValidAmount, measurement);

            result.AssertFirstError(nameof(Quantity.Measurement), nameof(LengthValidator));
        }
    }
}