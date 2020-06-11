using FluentValidation;

namespace SocialChef.Domain.Recipes
{
    internal class IngredientValidator : AbstractValidator<Ingredient>
    {
        internal IngredientValidator()
        {
            RuleFor(i => i.Name).SetValidator(new IngredientNameValidator());
            RuleFor(i => i.Quantity).SetValidator(new QuantityValidator());
        }
    }
}