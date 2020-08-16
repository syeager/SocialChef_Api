using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LittleByte.Asp.Business;
using SocialChef.Domain.Recipes;

namespace SocialChef.Application.Dtos
{
    public sealed class RecipeDto : Dto
    {
        public Guid ChefID { get; [UsedImplicitly] set; }
        public string Name { get; [UsedImplicitly] set; }
        public Guid? VariantId { get; [UsedImplicitly] set; }
        public List<SectionDto> Sections { get; [UsedImplicitly] set; }

        [UsedImplicitly]
        public RecipeDto()
            : base(Guid.Empty)
        {
            Name = null!;
            Sections = null!;
        }

        public RecipeDto(Guid id, Guid chefID, string name, Guid variantId, List<SectionDto> sections)
            : base(id)
        {
            ChefID = chefID;
            Name = name;
            VariantId = variantId;
            Sections = sections;
        }

        public static implicit operator Recipe(RecipeDto recipeDto)
        {
            return Recipe.Construct(
                    recipeDto.ID != Guid.Empty ? recipeDto.ID : (Guid?)null,
                    recipeDto.ChefID,
                    recipeDto.Name,
                    recipeDto.VariantId,
                    recipeDto.Sections?
                        .Select(section => new Section(
                            new SectionName(section.Name),
                            section.Steps.Select(step => new Step(step.Instruction ?? string.Empty)
                            ).ToList())
                        ).ToList() ?? new List<Section>())
                .GetModelOrThrow();
        }

        public static implicit operator RecipeDto(Recipe recipe)
        {
            var dto = new RecipeDto(
                recipe.ID.Value,
                recipe.ChefID.Value,
                recipe.Name,
                recipe.VariantId.Value,
                recipe.Sections.Select(s => new SectionDto(
                    s.Name,
                    s.Steps.Select(step => new StepDto(step.Instruction))
                        .ToList())
                ).ToList());
            return dto;
        }
    }
}