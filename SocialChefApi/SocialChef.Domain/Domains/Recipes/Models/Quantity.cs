using FluentValidation;
using JetBrains.Annotations;
using LittleByte.Domain;

namespace SocialChef.Domain.Recipes
{
    internal class Quantity
    {
        public class Validator : AbstractValidator<Quantity>
        {
            public const int AmountMinExclusive = 0;
            public const int AmountMax = 1000;
            public const int MeasurementLengthMin = 1;
            public const int MeasurementLengthMax = 50;

            public static readonly Validator Instance = new Validator();

            private Validator()
            {
                RuleFor(q => q.Amount).ExclusiveBetween(AmountMinExclusive, AmountMax);
                RuleFor(q => q.Measurement).Length(MeasurementLengthMin, MeasurementLengthMax);
            }
        }

        public decimal Amount { get; }
        public string Measurement { get; }

        [UsedImplicitly]
        private Quantity()
        {
            Amount = 0m;
            Measurement = null!;
        }

        public Quantity(decimal amount, string measurement)
        {
            Amount = amount;
            Measurement = measurement.Trim();
        }

        public static ModelConstructResult<Quantity> Construct(decimal amount, string measurement)
        {
            var quantity = new Quantity(amount, measurement);

            var validation = Validator.Instance.Validate(quantity);

            return ModelConstructResult<Quantity>.Construct(quantity, validation);
        }
    }
}