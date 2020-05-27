using FluentValidation;

namespace SocialChef.Domain.Chefs
{
    public class ChefNameValidator : AbstractValidator<ChefName>
    {
        public const int NameLengthMin = 5;
        public const int NameLengthMax = 50;

        public ChefNameValidator()
        {
            RuleFor(cn => cn.Value).Length(NameLengthMin, NameLengthMax);
        }
    }
}