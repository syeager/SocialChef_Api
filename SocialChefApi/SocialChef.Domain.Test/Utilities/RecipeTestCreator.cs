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
                ValidProperties.RecipeName,
                DomainGuid<Recipe>.Empty,
                new[]
                {
                    new Section(
                        ValidProperties.SectionName,
                        new[]
                        {
                            new Step(ValidProperties.StepName),
                        })
                });
            return recipe;
        }
    }
}