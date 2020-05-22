using System;
using System.Collections.Generic;
using FluentValidation.Validators;
using LittleByte.Domain.Test.Utilities;
using NUnit.Framework;
using SocialChef.Domain.Recipes;

namespace LittleByte.Domain.Test.Models
{
    public class RecipeTest
    {
        private static readonly Recipe.Guid? validID = new Recipe.Guid(Guid.NewGuid());
        private static readonly Chef.Guid validChefID = new Chef.Guid(Guid.NewGuid());
        private static readonly IReadOnlyList<Section> validSections = new[] {new Section("name", new[] {new Step("instruction"),})};

        [Test]
        public void Construct_Valid_Success()
        {
            var results = Recipe.Construct(validID, validChefID, ValidModelValues.RecipeName, Recipe.Guid.Empty, validSections);

            Assert.IsTrue(results.IsSuccess);
        }

        [Test]
        public void Construct_ChefIDEmpty_ValidationError()
        {
            var chefId = new Chef.Guid(Guid.Empty);

            var results = Recipe.Construct(validID, chefId, ValidModelValues.RecipeName, Recipe.Guid.Empty, validSections);

            results.AssertFirstError(nameof(Recipe.ChefID), nameof(NotEqualValidator));
        }

        [TestCase(Recipe.Validator.NameLengthMin)]
        [TestCase(Recipe.Validator.NameLengthMax)]
        public void Construct_NameLengthValid_Success(int charCount)
        {
            var name = new string('a', charCount);

            var results = Recipe.Construct(validID, validChefID, name, Recipe.Guid.Empty, validSections);

            Assert.IsTrue(results.IsSuccess);
        }

        [TestCase('a', Recipe.Validator.NameLengthMin - 1)]
        [TestCase(' ', Recipe.Validator.NameLengthMin)]
        [TestCase('a', Recipe.Validator.NameLengthMax + 1)]
        public void Construct_NameLengthInvalid_ValidationError(char character, int count)
        {
            var name = new string(character, count);

            var results = Recipe.Construct(validID, validChefID, name, Recipe.Guid.Empty, validSections);

            results.AssertFirstError(nameof(Recipe.Name), nameof(LengthValidator));
        }

        [Test]
        public void Construct_SectionsEmpty_ValidationError()
        {
            var sections = Array.Empty<Section>();

            var results = Recipe.Construct(validID, validChefID, ValidModelValues.RecipeName, Recipe.Guid.Empty, sections);

            results.AssertFirstError(nameof(Recipe.Sections), nameof(NotEmptyValidator));
        }

        [Test]
        public void Construct_IDNull_NewID()
        {
            var results = Recipe.Construct(null, validChefID, ValidModelValues.RecipeName, Recipe.Guid.Empty, validSections);

            Assert.AreNotEqual(Guid.Empty, results.Model!.ID);
        }

        [Test]
        public void Construct_VariantNotEmpty_VariantIdSet()
        {
            Recipe.Guid variantId  = new Recipe.Guid(Guid.NewGuid());

            var results = Recipe.Construct(null, validChefID, ValidModelValues.RecipeName, variantId, validSections);

            Assert.AreEqual(variantId.Value, results.GetModelOrThrow().VariantId.Value);
        }
    }
}