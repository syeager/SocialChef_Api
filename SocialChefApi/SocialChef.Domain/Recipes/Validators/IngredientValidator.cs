using FluentValidation;

namespace SocialChef.Domain.Recipes
{
    internal class IngredientValidator : AbstractValidator<Ingredient>
    {
        public const int NameLengthMin = 1;
        public const int NameLengthMax = 50;

        internal IngredientValidator()
        {
            RuleFor(i => i.Name).Length(NameLengthMin, NameLengthMax);
            RuleFor(i => i.Quantity).SetValidator(new QuantityValidator());
        }
    }
}