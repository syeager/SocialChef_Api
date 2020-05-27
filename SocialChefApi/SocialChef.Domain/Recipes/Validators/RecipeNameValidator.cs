using FluentValidation;

namespace SocialChef.Domain.Recipes
{
    internal class RecipeNameValidator : AbstractValidator<RecipeName>
    {
        public const int LengthMin = 1;
        public const int LengthMax = 50;

        public RecipeNameValidator()
        {
            RuleFor(r => r.Value).Length(LengthMin, LengthMax);
        }
    }
}