using FluentValidation;

namespace SocialChef.Domain.Recipes
{
    internal class SectionNameValidator : AbstractValidator<SectionName>
    {
        public const int LengthMin = 1;
        public const int LengthMax = 50;

        internal SectionNameValidator()
        {
            RuleFor(sn => sn.Value).Length(LengthMin, LengthMax);
        }
    }
}