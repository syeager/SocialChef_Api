using System;
using FluentValidation.Validators;
using LittleByte.Asp.Test.Utilities;
using NUnit.Framework;
using SocialChef.Domain.Recipes;
using SocialChef.Domain.Test.Utilities;

namespace SocialChef.Domain.Test.Domains.Recipes.Validators
{
    public class RecipeSummaryValidatorTest
    {
        private RecipeSummaryValidator testObj;

        [SetUp]
        public void SetUp()
        {
            testObj = new RecipeSummaryValidator();
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Validate_StepCount0OrLess_FailWithRange(int count)
        {
            var recipeSummary = new RecipeSummary(Guid.NewGuid(), Guid.NewGuid(), ValidProperties.RecipeName, count);

            var result = testObj.Validate(recipeSummary);

            result.AssertFirstError(nameof(RecipeSummary.StepCount), nameof(GreaterThanValidator));
        }
    }
}