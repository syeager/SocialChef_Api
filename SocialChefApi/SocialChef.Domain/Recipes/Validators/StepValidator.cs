using FluentValidation;

namespace SocialChef.Domain.Recipes
{
    internal class StepValidator : AbstractValidator<Step>
    {
        public const int NameLengthMin = 1;
        public const int NameLengthMax = 50;

        internal StepValidator()
        {
            RuleFor(s => s.Instruction).Length(NameLengthMin, NameLengthMax);
        }
    }
}