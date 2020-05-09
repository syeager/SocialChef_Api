using System;
using System.Collections.Generic;
using FluentValidation.Validators;
using LittleByte.Domain.Test.Utilities;
using NUnit.Framework;
using SocialChef.Domain.Recipes;

namespace LittleByte.Domain.Test.Models
{
    public class SectionTest
    {
        private static readonly IReadOnlyList<Step> validSteps = new[] {Step.Empty};

        [Test]
        public void Construct_Valid_Success()
        {
            var results = Section.Construct(ValidModelValues.SectionName, validSteps);

            Assert.IsTrue(results.IsSuccess);
        }

        [TestCase("")]
        [TestCase(" ")]
        public void Construct_NameTooShort_ValidationError(string name)
        {
            var results = Section.Construct(name, validSteps);

            results.AssertFirstError(nameof(Section.Name), nameof(LengthValidator));
        }

        [Test]
        public void Construct_NameTooLong_ValidationError()
        {
            var name = new string('a', Section.Validator.NameLengthMax + 1);

            var results = Section.Construct(name, validSteps);

            results.AssertFirstError(nameof(Section.Name), nameof(LengthValidator));
        }

        [TestCase(Section.Validator.NameLengthMin)]
        [TestCase(Section.Validator.NameLengthMax)]
        public void Construct_NameValid_Success(int charCount)
        {
            var name = new string('a', charCount);

            var results = Section.Construct(name, validSteps);

            Assert.IsTrue(results.IsSuccess);
        }

        [Test]
        public void Construct_StepsEmpty_ValidationError()
        {
            var steps = Array.Empty<Step>();

            var results = Section.Construct(ValidModelValues.SectionName, steps);

            results.AssertFirstError(nameof(Section.Steps), nameof(NotEmptyValidator));
        }
    }
}