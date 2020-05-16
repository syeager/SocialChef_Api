using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using LittleByte.Domain;
using SocialChef.Domain.Document;

namespace SocialChef.Domain.Recipes
{
    public class Recipe
    {
        public class Validator : AbstractValidator<Recipe>
        {
            public const int NameLengthMin = 1;
            public const int NameLengthMax = 50;

            public static Validator Instance { get; } = new Validator();

            private Validator()
            {
                RuleFor(r => r.Name).Length(NameLengthMin, NameLengthMax);
                RuleFor(r => r.Sections).NotEmpty();
                RuleFor(r => r.ChefID).NotEqual(new Chef.Guid(System.Guid.Empty));
                RuleForEach(r => r.Sections).SetValidator(Section.Validator.Instance);
            }
        }

        public readonly struct Guid
        {
            public System.Guid Value { get; }

            public Guid(System.Guid value)
            {
                Value = value;
            }
        }

        public Guid ID { get; }
        public Chef.Guid ChefID { get; }
        public string Name { get; }
        public IReadOnlyList<Section> Sections { get; }

        private Recipe(Guid id, Chef.Guid chefID, string name, IReadOnlyList<Section> sections)
        {
            ID = id;
            ChefID = chefID;
            Name = name.Trim();
            Sections = sections;
        }

        public static ModelConstructResult<Recipe> Construct(Guid? id, Chef.Guid chefID, string name, IReadOnlyList<Section> sections)
        {
            id ??= new Guid(System.Guid.NewGuid());
            var recipe = new Recipe(id.Value, chefID, name, sections);

            var validation = Validator.Instance.Validate(recipe);

            return ModelConstructResult<Recipe>.Construct(recipe, validation);
        }

        public static implicit operator RecipeDao(Recipe recipe)
        {
            int stepCount = 0;
            return new RecipeDao(
                recipe.ID.Value,
                recipe.ChefID.Value,
                recipe.Name,
                recipe.Sections.Select(section => new SectionDao(
                        section.Name,
                        section.Steps.Select(step => new StepDao(++stepCount, step.Instruction)
                        ).ToList()
                    )
                ).ToList()
            );
        }

        public static implicit operator Recipe(RecipeDao recipeDao)
        {
            return Construct(
                new Guid(recipeDao.ID),
                new Chef.Guid(recipeDao.ChefID),
                recipeDao.Name,
                recipeDao.Sections
                    .Select(section => new Section(
                            section.Name,
                            section.Steps.Select(step => new Step(step.Instruction)
                            ).ToList()
                        )
                    ).ToList()
            ).GetModelOrThrow();
        }
    }
}