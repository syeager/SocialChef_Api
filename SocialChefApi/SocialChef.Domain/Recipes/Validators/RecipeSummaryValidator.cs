using FluentValidation;
using SocialChef.Domain.Chefs;

namespace SocialChef.Domain.Recipes
{
    public class RecipeSummaryValidator : AbstractValidator<RecipeSummary>
    {
        public RecipeSummaryValidator()
        {
            RuleFor(rs => rs.RecipeId).NotEqual(DomainGuid<Recipe>.Empty);
            RuleFor(rs => rs.Name).SetValidator(new RecipeNameValidator());
            RuleFor(rs => rs.ChefId).NotEqual(DomainGuid<Chef>.Empty);
            RuleFor(rs => rs.StepCount).GreaterThan(0);
        }
    }
}