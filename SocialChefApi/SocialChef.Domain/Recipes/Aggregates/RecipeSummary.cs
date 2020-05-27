using System;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Relational;

namespace SocialChef.Domain.Recipes
{
    public class RecipeSummary
    {
        public DomainGuid<Recipe> RecipeId { get; }
        public RecipeName Name { get; }
        public DomainGuid<Chef> ChefId { get; }
        public int StepCount { get; }

        internal RecipeSummary(DomainGuid<Recipe> recipeId, DomainGuid<Chef> chefId, RecipeName name, int stepCount)
        {
            RecipeId = recipeId;
            ChefId = chefId;
            Name = name;
            StepCount = stepCount;
        }

        public static ModelConstructResult<RecipeSummary> Construct(Guid recipeId, string recipeName, Guid chefId, int stepCount)
        {
            var recipeSummary = new RecipeSummary(
                new DomainGuid<Recipe>(recipeId),
                new DomainGuid<Chef>(chefId),
                new RecipeName(recipeName),
                stepCount);

            var validation = new RecipeSummaryValidator().Validate(recipeSummary);

            return ModelConstructResult<RecipeSummary>.Construct(recipeSummary, validation);
        }

        public static implicit operator RecipeSummaryDao(RecipeSummary recipeSummary)
        {
            return new RecipeSummaryDao(
                recipeSummary.RecipeId.Value,
                recipeSummary.Name,
                recipeSummary.ChefId.Value,
                recipeSummary.StepCount);
        }

        public static implicit operator RecipeSummary(RecipeSummaryDao recipeSummaryDao)
        {
            return Construct(
                    new DomainGuid<Recipe>(recipeSummaryDao.ID),
                    new RecipeName(recipeSummaryDao.RecipeName),
                    new DomainGuid<Chef>(recipeSummaryDao.ChefId),
                    recipeSummaryDao.StepCount)
                .GetModelOrThrow();
        }
    }
}