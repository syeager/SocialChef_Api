using System;
using JetBrains.Annotations;
using LittleByte.Asp.Database;

namespace SocialChef.Domain.Relational
{
    public class RecipeSummaryDao : Entity
    {
        public string RecipeName { get; set; }
        public Guid ChefId { get; set; }
        public int StepCount { get; set; }

        [UsedImplicitly]
        public RecipeSummaryDao()
        {
            RecipeName = null!;
        }

        public RecipeSummaryDao(Guid recipeId, string recipeName, Guid chefId, int stepCount)
        {
            ID = recipeId;
            RecipeName = recipeName;
            ChefId = chefId;
            StepCount = stepCount;
        }
    }
}