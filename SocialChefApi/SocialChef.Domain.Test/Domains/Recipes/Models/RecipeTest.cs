using System;
using System.Collections.Generic;
using FluentValidation.Validators;
using NUnit.Framework;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Recipes;
using SocialChef.Domain.Test.Utilities;

namespace SocialChef.Domain.Test.Domains.Recipes.Models
{
    public class RecipeTest
    {
        private static readonly DomainGuid<Recipe>? validID = new DomainGuid<Recipe>(Guid.NewGuid());
        private static readonly DomainGuid<Chef> validChefID = new DomainGuid<Chef>(Guid.NewGuid());
        private static readonly IReadOnlyList<Section> validSections = new[] {new Section(ValidProperties.SectionName, new[] {new Step("instruction"),})};

        [Test]
        public void Construct_Valid_Success()
        {
            var results = Recipe.Construct(validID, validChefID, ValidProperties.RecipeName, DomainGuid<Recipe>.Empty, validSections);

            Assert.IsTrue(results.IsSuccess);
        }

        [Test]
        public void Construct_ChefIDEmpty_ValidationError()
        {
            var chefId = Guid.Empty;

            var results = Recipe.Construct(validID, chefId, ValidProperties.RecipeName, DomainGuid<Recipe>.Empty, validSections);

            results.AssertFirstError(nameof(Recipe.ChefID), nameof(NotEqualValidator));
        }

        [Test]
        public void Construct_SectionsEmpty_ValidationError()
        {
            var sections = Array.Empty<Section>();

            var results = Recipe.Construct(validID, validChefID, ValidProperties.RecipeName, DomainGuid<Recipe>.Empty, sections);

            results.AssertFirstError(nameof(Recipe.Sections), nameof(NotEmptyValidator));
        }

        [Test]
        public void Construct_IDNull_NewID()
        {
            var results = Recipe.Construct(null, validChefID, ValidProperties.RecipeName, DomainGuid<Recipe>.Empty, validSections);

            Assert.AreNotEqual(Guid.Empty, results.Model!.ID);
        }

        [Test]
        public void Construct_VariantNotEmpty_VariantIdSet()
        {
            var variantId = Guid.NewGuid();

            var results = Recipe.Construct(null, validChefID, ValidProperties.RecipeName, variantId, validSections);

            Assert.AreEqual(variantId, results.GetModelOrThrow().VariantId.Value);
        }
    }
}