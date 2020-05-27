using System;
using JetBrains.Annotations;
using LittleByte.Asp.Business;
using SocialChef.Domain.Recipes;

namespace SocialChef.Application.Dtos.Recipes
{
    public sealed class RecipeSummaryDto : Dto
    {
        public string RecipeName { get; [UsedImplicitly] set; }
        public Guid ChefId { get; [UsedImplicitly] set; }
        public int StepCount { get; set; }

        [UsedImplicitly]
        public RecipeSummaryDto()
            : base(Guid.Empty)
        {
            RecipeName = null!;
        }

        public RecipeSummaryDto(Guid id, string recipeName, Guid chefId, int stepCount)
            : base(id)
        {
            RecipeName = recipeName;
            ChefId = chefId;
            StepCount = stepCount;
        }

        public static implicit operator RecipeSummary(RecipeSummaryDto recipeSummaryDto)
        {
            return RecipeSummary.Construct(
                    recipeSummaryDto.ID,
                    recipeSummaryDto.RecipeName,
                    recipeSummaryDto.ChefId,
                    recipeSummaryDto.StepCount
                    )
                .GetModelOrThrow();
        }

        public static implicit operator RecipeSummaryDto(RecipeSummary recipeSummary)
        {
            return new RecipeSummaryDto(
                recipeSummary.RecipeId.Value,
                recipeSummary.Name,
                recipeSummary.ChefId.Value,
                recipeSummary.StepCount);
        }
    }
}