using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using JetBrains.Annotations;
using LittleByte.Asp.Business;
using SocialChef.Domain.Recipes;

namespace SocialChef.Application.Dtos.Recipes
{
    public sealed class RecipeDto : Dto
    {
        public Guid ChefID { get; [UsedImplicitly] set; }
        public string Name { get; [UsedImplicitly] set; }
        public Guid? VariantId { get; [UsedImplicitly] set; }
        public List<IngredientDto> Ingredients { get; [UsedImplicitly] set; }
        public List<SectionDto> Sections { get; [UsedImplicitly] set; }

        [UsedImplicitly]
        public RecipeDto()
            : base(Guid.Empty)
        {
            Name = null!;
            Ingredients = null!;
            Sections = null!;
        }

        public RecipeDto(Guid id, Guid chefID, string name, Guid variantId, List<IngredientDto> ingredients, List<SectionDto> sections)
            : base(id)
        {
            ChefID = chefID;
            Name = name;
            VariantId = variantId;
            Ingredients = ingredients;
            Sections = sections;
        }

        public static implicit operator Recipe(RecipeDto recipeDto)
        {
            return Recipe.Construct(
                    recipeDto.ID != Guid.Empty ? recipeDto.ID : (Guid?)null,
                    recipeDto.ChefID,
                    recipeDto.Name,
                    recipeDto.VariantId,
                    recipeDto.Ingredients?.Cast<Ingredient>().ToArray() ?? Array.Empty<Ingredient>(),
                    recipeDto.Sections?
                        .Select(section => new Section(
                            new SectionName(section.Name),
                            section.Steps.Select(step => new Step(step.Instruction ?? string.Empty)
                            ).ToArray())
                        ).ToArray() ?? Array.Empty<Section>())
                .GetModelOrThrow();
        }

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public static implicit operator RecipeDto(Recipe recipe)
        {
            var dto = new RecipeDto(
                recipe.ID.Value,
                recipe.ChefID.Value,
                recipe.Name,
                recipe.VariantId.Value,
                recipe.Ingredients.Cast<IngredientDto>().ToList(),
                recipe.Sections.Select(s => new SectionDto(
                    s.Name,
                    s.Steps.Select(step => new StepDto(step.Instruction))
                        .ToList())
                ).ToList());
            return dto;
        }
    }
}