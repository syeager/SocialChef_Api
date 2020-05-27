using System;
using System.Collections.Generic;
using FluentValidation.Validators;
using LittleByte.Asp.Test.Utilities;
using NUnit.Framework;
using SocialChef.Domain.Recipes;
using SocialChef.Domain.Test.Utilities;

namespace SocialChef.Domain.Test.Domains.Recipes.Validators
{
    public class SectionValidatorTest
    {
        private static readonly IReadOnlyList<Step> validSteps = new[] {new Step(ValidProperties.StepName)};

        private SectionValidator testObj;

        [SetUp]
        public void SetUp()
        {
            testObj = new SectionValidator();
        }

        [Test]
        public void Construct_Valid_Success()
        {
            var section = new Section(ValidProperties.SectionName, validSteps);

            var results = testObj.Validate(section);

            Assert.IsTrue(results.IsValid);
        }

        [Test]
        public void Construct_StepsEmpty_ValidationError()
        {
            var steps = Array.Empty<Step>();
            var section = new Section(ValidProperties.SectionName, steps);

            var results = testObj.Validate(section);

            results.AssertFirstError(nameof(Section.Steps), nameof(NotEmptyValidator));
        }
    }
}