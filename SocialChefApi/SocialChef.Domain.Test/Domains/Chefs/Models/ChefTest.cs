using System;
using FluentValidation.Validators;
using NUnit.Framework;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Identity;
using SocialChef.Domain.Test.Utilities;

namespace SocialChef.Domain.Test.Domains.Chefs.Models
{
    public class ChefTest
    {
        private static readonly DomainGuid<Chef> validID = new DomainGuid<Chef>(Guid.NewGuid());
        private static readonly DomainGuid<User> validChefId = new DomainGuid<User>(Guid.NewGuid());

        [Test]
        public void Construct_Valid_Success()
        {
            var results = Chef.Construct(validID.Value, validChefId, ValidProperties.ChefName);

            Assert.IsTrue(results.IsSuccess);
        }

        [Test]
        public void Construct_IDNull_NewID()
        {
            var results = Chef.Construct(null, validChefId, ValidProperties.ChefName);

            Assert.IsTrue(results.IsSuccess);
            Assert.AreNotEqual(Guid.Empty, results.Model!.ID);
        }

        [Test]
        public void Construct_EmptyUser_ValidationError()
        {
            var userId = new DomainGuid<User>(Guid.Empty);
            var results = Chef.Construct(null, userId, ValidProperties.ChefName);

            results.AssertFirstError(nameof(Chef.UserId), nameof(NotEmptyValidator));
        }
    }
}