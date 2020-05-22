using System.Collections.Generic;
using FluentValidation;
using JetBrains.Annotations;
using LittleByte.Domain;

namespace SocialChef.Domain.Recipes
{
    public class Section
    {
        public class Validator : AbstractValidator<Section>
        {
            public const int NameLengthMin = 1;
            public const int NameLengthMax = 50;

            public static readonly Validator Instance = new Validator();

            private Validator()
            {
                RuleFor(s => s.Name).Length(NameLengthMin, NameLengthMax);
                RuleFor(s => s.Steps).NotEmpty();
                RuleForEach(s => s.Steps).SetValidator(Step.Validator.Instance);
            }
        }

        public string Name { get; }
        public IReadOnlyList<Step> Steps { get; }

        [UsedImplicitly]
        private Section()
        {
            Name = null!;
            Steps = null!;
        }

        public Section(string name, IReadOnlyList<Step> steps)
        {
            Name = name.Trim();
            Steps = steps;
        }

        public static ModelConstructResult<Section> Construct(string name, IReadOnlyList<Step> steps)
        {
            var section = new Section(name, steps);

            var validation = Validator.Instance.Validate(section);

            return ModelConstructResult<Section>.Construct(section, validation);
        }
    }
}