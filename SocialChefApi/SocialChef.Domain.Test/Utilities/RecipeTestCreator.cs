using System;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Utilities
{
    public static class RecipeTestCreator
    {
        public static ModelConstructResult<Recipe> RecipeWithNewGuid(Guid? chefId = null) => CreateRecipe(Guid.NewGuid(), chefId);
        public static ModelConstructResult<Recipe> RecipeWithEmptyGuid(Guid? chefId = null) => CreateRecipe(Guid.Empty, chefId);

        private static ModelConstructResult<Recipe> CreateRecipe(Guid recipeId, Guid? chefId = null)
        {
            var id = new DomainGuid<Recipe>(recipeId);
            var recipe = Recipe.Construct(
                id,
                new DomainGuid<Chef>(chefId ?? Guid.NewGuid()),
                Valid.RecipeName,
                DomainGuid<Recipe>.Empty,
                new[]
                {
                    new Section(
                        Valid.SectionName,
                        new[]
                        {
                            new Step(Valid.StepName),
                        })
                });
            return recipe;
        }
    }
}