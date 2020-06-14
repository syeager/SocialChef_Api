using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        public IReadOnlyList<Ingredient> Ingredients { get; }
        public IReadOnlyList<Section> Sections { get; }

        private Recipe(DomainGuid<Recipe> id, DomainGuid<Chef> chefID, RecipeName name, DomainGuid<Recipe> variantId, IReadOnlyList<Ingredient> ingredients, IReadOnlyList<Section> sections)
        {
            ID = id;
            ChefID = chefID;
            Name = name;
            VariantId = variantId;
            Ingredients = ingredients;
            Sections = sections;
        }

        public static ModelConstructResult<Recipe> Construct(Guid? id, Guid chefID, string name, Guid? variantId, IReadOnlyList<Ingredient> ingredients, IReadOnlyList<Section> sections)
        {
            var recipe = new Recipe(id, chefID, new RecipeName(name), variantId, ingredients, sections);

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
                recipe.Ingredients.Select(i => (IngredientDao)i).ToArray(),
                recipe.Sections.Select(section => new SectionDao(
                        section.Name,
                        section.Steps.Select(step => new StepDao(++stepCount, step.Instruction)
                        ).ToArray()
                    )
                ).ToArray()
            );
        }

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public static implicit operator Recipe(RecipeDao recipeDao)
        {
            return Construct(
                recipeDao.ID,
                recipeDao.ChefID,
                new RecipeName(recipeDao.Name),
                recipeDao.VariantId,
                recipeDao.Ingredients.Select(i => (Ingredient)i).ToArray(),
                recipeDao.Sections
                    .Select(section => new Section(
                            new SectionName(section.Name),
                            section.Steps.Select(step => new Step(step.Instruction)
                            ).ToArray()
                        )
                    ).ToArray()
            ).GetModelOrThrow();
        }
    }
}