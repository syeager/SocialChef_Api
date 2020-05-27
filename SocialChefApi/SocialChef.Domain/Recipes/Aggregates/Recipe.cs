using System;
using System.Collections.Generic;
using System.Linq;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Document;

namespace SocialChef.Domain.Recipes
{
    public class Recipe
    {
        public DomainGuid<Recipe> ID { get; }
        public DomainGuid<Chef> ChefID { get; }
        public DomainGuid<Recipe> VariantId { get; }
        public RecipeName Name { get; }
        public IReadOnlyList<Section> Sections { get; }

        private Recipe(DomainGuid<Recipe> id, DomainGuid<Chef> chefID, RecipeName name, DomainGuid<Recipe> variantId, IReadOnlyList<Section> sections)
        {
            ID = id;
            ChefID = chefID;
            Name = name;
            VariantId = variantId;
            Sections = sections;
        }

        public static ModelConstructResult<Recipe> Construct(Guid? id, Guid chefID, string name, Guid? variantId, IReadOnlyList<Section> sections)
        {
            var recipe = new Recipe(id, chefID, new RecipeName(name), variantId, sections);

            var validation = RecipeValidator.Instance.Validate(recipe);

            return ModelConstructResult<Recipe>.Construct(recipe, validation);
        }

        public static implicit operator RecipeDao(Recipe recipe)
        {
            var stepCount = 0;
            return new RecipeDao(
                recipe.ID.Value,
                recipe.ChefID.Value,
                recipe.Name,
                recipe.VariantId.Value,
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
                recipeDao.ID,
                recipeDao.ChefID,
                new RecipeName(recipeDao.Name),
                recipeDao.VariantId,
                recipeDao.Sections
                    .Select(section => new Section(
                            new SectionName(section.Name),
                            section.Steps.Select(step => new Step(step.Instruction)
                            ).ToList()
                        )
                    ).ToList()
            ).GetModelOrThrow();
        }
    }
}