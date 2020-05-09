using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LittleByte.Asp.Business;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.DTOs
{
    public sealed class RecipeDto : Dto
    {
        public Guid ChefID { get; [UsedImplicitly] set; }
        public string Name { get; [UsedImplicitly] set; }
        public List<SectionDto> Sections { get; [UsedImplicitly] set; }

        [UsedImplicitly]
        public RecipeDto()
            : base(Guid.Empty)
        {
            Name = null!;
            Sections = null!;
        }

        public RecipeDto(Guid id, Guid chefID, string name, List<SectionDto> sections)
            : base(id)
        {
            ChefID = chefID;
            Name = name;
            Sections = sections;
        }

        public static implicit operator Recipe(RecipeDto recipeDto)
        {
            return Recipe.Construct(
                    recipeDto.ID != Guid.Empty ? new Recipe.Guid(recipeDto.ID) : (Recipe.Guid?)null,
                    new Chef.Guid(recipeDto.ChefID),
                    recipeDto.Name,
                    recipeDto.Sections
                        .Select(section => new Section(
                            section.Name,
                            section.Steps.Select(step => new Step(step.Instruction)
                            ).ToList())
                        ).ToList())
                .GetModelOrThrow();
        }

        public static implicit operator RecipeDto(Recipe recipe)
        {
            var dto = new RecipeDto(
                recipe.ID.Value,
                recipe.ChefID.Value,
                recipe.Name,
                recipe.Sections.Select(s => new SectionDto(
                    s.Name,
                    s.Steps.Select(step => new StepDto(step.Instruction))
                        .ToList())
                ).ToList());
            return dto;
        }
    }
}