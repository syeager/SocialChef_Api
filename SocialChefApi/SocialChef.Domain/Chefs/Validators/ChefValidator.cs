using FluentValidation;

namespace SocialChef.Domain.Chefs
{
    internal class ChefValidator : AbstractValidator<Chef>
    {
        public ChefValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            // TODO: Unit test.
            RuleFor(c => c.Name).SetValidator(new ChefNameValidator());
        }
    }
}