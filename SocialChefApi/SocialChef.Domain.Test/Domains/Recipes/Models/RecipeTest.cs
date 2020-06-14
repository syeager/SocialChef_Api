using System;
using FluentValidation.Validators;
using NUnit.Framework;
using SocialChef.Domain.Recipes;
using SocialChef.Domain.Test.Utilities;

namespace SocialChef.Domain.Test.Domains.Recipes.Models
{
    public class RecipeTest
    {
        [Test]
        public void Construct_Valid_Success()
        {
            var results = Valid.RecipeProps.Create(Valid.RecipeProps.Id);

            Assert.IsTrue(results.IsSuccess);
        }

        [Test]
        public void Construct_ChefIDEmpty_ValidationError()
        {
            var chefId = Guid.Empty;

            var results = Valid.RecipeProps.Create(Valid.RecipeProps.Id, chefId);

            results.AssertFirstError(nameof(Recipe.ChefID), nameof(NotEqualValidator));
        }

        [Test]
        public void Construct_SectionsEmpty_ValidationError()
        {
            var sections = Array.Empty<Section>();

            var results = Valid.RecipeProps.Create(Valid.RecipeProps.Id, sections: sections);

            results.AssertFirstError(nameof(Recipe.Sections), nameof(NotEmptyValidator));
        }

        [Test]
        public void Construct_IDNull_NewID()
        {
            var results = Valid.RecipeProps.Create(null);

            Assert.AreNotEqual(Guid.Empty, results.Model!.ID);
        }

        [Test]
        public void Construct_VariantNotEmpty_VariantIdSet()
        {
            var variantId = Guid.NewGuid();

            var results = Valid.RecipeProps.Create(Valid.RecipeProps.Id, variantId: variantId);

            Assert.AreEqual(variantId, results.GetModelOrThrow().VariantId.Value);
        }
    }
}