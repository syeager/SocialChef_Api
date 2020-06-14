using System;
using System.Collections.Generic;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Utilities
{
    public static partial class Valid
    {
        public static class RecipeProps
        {
            public static readonly DomainGuid<Recipe>? Id = new DomainGuid<Recipe>(Guid.NewGuid());
            public static readonly DomainGuid<Chef> ChefId = new DomainGuid<Chef>(Guid.NewGuid());
            public static readonly RecipeName Name = new RecipeName(new string('a', RecipeNameValidator.LengthMin));
            public static readonly Ingredient[] Ingredients = { IngredientProps.Ingredient };
            public static readonly IReadOnlyList<Section> Sections = new[] { new Section(Valid.SectionName, new[] { new Step("instruction"), }) };

            public static ModelConstructResult<Recipe> Create(Guid? id,
                Guid? chefId = null,
                string? name = null,
                Guid? variantId = null,
                IReadOnlyList<Ingredient>? ingredients = null,
                IReadOnlyList<Section>? sections = null)
            {
                return Recipe.Construct(id,
                    chefId ?? ChefId,
                    name ?? Name,
                    variantId,
                    ingredients ?? Ingredients,
                    sections ?? Sections);
            }
        }
    }
}