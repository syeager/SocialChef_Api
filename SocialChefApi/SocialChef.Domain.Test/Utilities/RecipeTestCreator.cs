using System;
using SocialChef.Domain.Recipes;

namespace LittleByte.Domain.Test.Utilities
{
    public static class RecipeTestCreator
    {
        public static ModelConstructResult<Recipe> RecipeWithNewGuid(Guid? chefId = null) => CreateRecipe(Guid.NewGuid(), chefId);
        public static ModelConstructResult<Recipe> RecipeWithEmptyGuid(Guid? chefId = null) => CreateRecipe(Guid.Empty, chefId);

        public static ModelConstructResult<Recipe> CreateRecipe(Guid recipeId, Guid? chefId = null)
        {
            var id = new Recipe.Guid(recipeId);
            var recipe = Recipe.Construct(
                id,
                new Chef.Guid(chefId ?? Guid.NewGuid()),
                ValidModelValues.RecipeName,
                new[]
                {
                    new Section(
                        ValidModelValues.SectionName,
                        new[]
                        {
                            new Step(ValidModelValues.StepName),
                        })
                });
            return recipe;
        }
    }
}