using FluentValidation;
using LittleByte.Domain;

namespace SocialChef.Domain.Recipes
{
    public class Step
    {
        public class Validator : AbstractValidator<Step>
        {
            public const int NameLengthMin = 1;
            public const int NameLengthMax = 50;

            public static Validator Instance { get; } = new Validator();

            private Validator()
            {
                RuleFor(s => s.Instruction).Length(NameLengthMin, NameLengthMax);
            }
        }

        public static readonly Step Empty = new Step();

        public string Instruction { get; }

        private Step()
        {
            Instruction = null!;
        }

        public Step(string instruction)
        {
            Instruction = instruction.Trim();
        }

        public static ModelConstructResult<Step> Construct(string instruction)
        {
            var step = new Step(instruction);

            var validation = Validator.Instance.Validate(step);

            return ModelConstructResult<Step>.Construct(step, validation);
        }
    }
}