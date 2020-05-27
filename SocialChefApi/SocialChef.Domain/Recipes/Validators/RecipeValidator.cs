using FluentValidation;
using SocialChef.Domain.Chefs;

namespace SocialChef.Domain.Recipes
{
    internal class RecipeValidator : AbstractValidator<Recipe>
    {
        public static RecipeValidator Instance { get; } = new RecipeValidator();

        private RecipeValidator()
        {
            RuleFor(rs => rs.Name).SetValidator(new RecipeNameValidator());
            RuleFor(r => r.Sections).NotEmpty();
            RuleFor(r => r.ChefID).NotEqual(DomainGuid<Chef>.Empty);
            RuleForEach(r => r.Sections).SetValidator(new SectionValidator());
        }
    }
}