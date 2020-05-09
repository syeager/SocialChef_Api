using FluentValidation;
using LittleByte.Domain;

namespace SocialChef.Domain.Recipes
{
    internal class Ingredient
    {
        public class Validator : AbstractValidator<Ingredient>
        {
            public const int NameLengthMin = 1;
            public const int NameLengthMax = 50;

            public static Validator Instance { get; } = new Validator();

            private Validator()
            {
                RuleFor(i => i.Name).Length(NameLengthMin, NameLengthMax);
                RuleFor(i => i.Quantity).SetValidator(Quantity.Validator.Instance);
            }
        }

        public string Name { get; }
        public Quantity Quantity { get; }

        private Ingredient(string name, Quantity quantity)
        {
            Quantity = quantity;
            Name = name.Trim();
        }

        public static ModelConstructResult<Ingredient> Construct(string name, Quantity quantity)
        {
            var ingredient = new Ingredient(name, quantity);

            var validation = Validator.Instance.Validate(ingredient);

            return ModelConstructResult<Ingredient>.Construct(ingredient, validation);
        }
    }
}