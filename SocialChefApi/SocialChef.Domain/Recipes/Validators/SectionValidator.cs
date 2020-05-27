using FluentValidation;

namespace SocialChef.Domain.Recipes
{
    internal class SectionValidator : AbstractValidator<Section>
    {
        internal SectionValidator()
        {
            RuleFor(s => s.Name).SetValidator(new SectionNameValidator());
            RuleFor(s => s.Steps).NotEmpty();
            RuleForEach(s => s.Steps).SetValidator(new StepValidator());
        }
    }
}