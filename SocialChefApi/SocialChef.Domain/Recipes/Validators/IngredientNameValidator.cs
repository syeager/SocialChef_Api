using FluentValidation;

namespace SocialChef.Domain.Recipes
{
    internal class IngredientNameValidator : AbstractValidator<IngredientName>
    {
        public const int LengthMin = 1;
        public const int LengthMax = 50;

        public IngredientNameValidator() => RuleFor(r => r.Value).Length(LengthMin, LengthMax);
    }
}