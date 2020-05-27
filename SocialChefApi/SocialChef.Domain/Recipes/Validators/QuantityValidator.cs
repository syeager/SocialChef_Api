using FluentValidation;

namespace SocialChef.Domain.Recipes
{
    internal class QuantityValidator : AbstractValidator<Quantity>
    {
        public const int AmountMinExclusive = 0;
        public const int AmountMax = 1000;
        public const int MeasurementLengthMin = 1;
        public const int MeasurementLengthMax = 50;

        internal QuantityValidator()
        {
            RuleFor(q => q.Amount).ExclusiveBetween(AmountMinExclusive, AmountMax);
            RuleFor(q => q.Measurement).Length(MeasurementLengthMin, MeasurementLengthMax);
        }
    }
}