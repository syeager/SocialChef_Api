using System;
using System.Linq;
using System.Threading.Tasks;
using LittleByte.Asp.Test.Database;
using LittleByte.Asp.Test.Utilities;
using NUnit.Framework;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Identity;
using SocialChef.Domain.Relational;
using SocialChef.Domain.Test.Utilities;

namespace SocialChef.Domain.Test.Domains.Chefs.Services
{
    public class ChefFinderTest
    {
        private ChefFinder testObj;
        private SqlDbContext sqlContext;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DbContextUtility.CreateInMemory(ref sqlContext);
        }

        [SetUp]
        public void SetUp()
        {
            sqlContext.EnsureRecreated();

            testObj = new ChefFinder(sqlContext);
        }

        [Test]
        public void FindByIdAsync_NoChef_ThrowNotFound()
        {
            var chefId = new DomainGuid<Chef>(Guid.Empty);

            AssertExtension.ThrowsNotFoundAsync<Chef>(() => testObj.FindByIdAsync(chefId), chefId.Value);
        }

        [Test]
        public async Task FindByIdAsync_HasChef_ReturnChef()
        {
            var chefDao = AddChefToDb();

            var chef = await testObj.FindByIdAsync(new DomainGuid<Chef>(chefDao.ID));

            Assert.AreEqual(chefDao.ID, chef.ID.Value);
        }

        [Test]
        public void FindByUserAsync_NoChef_ThrowNotFound()
        {
            var userId = new DomainGuid<User>(Guid.Empty);

            AssertExtension.ThrowsNotFoundAsync<User>(() => testObj.FindByUserAsync(userId), userId.Value);
        }

        [Test]
        public async Task FindByUserAsync_HasChef_ReturnChef()
        {
            var chefDao = AddChefToDb();

            var chef = await testObj.FindByUserAsync(new DomainGuid<User>(chefDao.UserID));

            Assert.AreEqual(chefDao.ID, chef.ID.Value);
        }

        // chef not found - skip?

        [Test]
        public async Task FindByIdsAsync_Valid_ReturnChefs()
        {
            var chef1 = AddChefToDb();
            var chef2 = AddChefToDb();

            var results = await testObj.FindByIdsAsync(new[] {chef1.ID, chef2.ID});

            Assert.AreEqual(2, results.Count);
            Assert.NotNull(results.Single(c => c.ID.Value == chef1.ID));
            Assert.NotNull(results.Single(c => c.ID.Value == chef2.ID));
        }

        [Test]
        public async Task FindByIdsAsync_EmptyList_ReturnEmpty()
        {
            var results = await testObj.FindByIdsAsync(Array.Empty<Guid>());

            Assert.NotNull(results);
            Assert.IsEmpty(results);
        }

        [Test]
        public async Task FindByIdsAsync_DuplicateId_DontReturnDuplicate()
        {
            var chef1 = AddChefToDb();
            AddChefToDb();

            var results = await testObj.FindByIdsAsync(new[] {chef1.ID, chef1.ID});

            Assert.AreEqual(1, results.Count);
            Assert.NotNull(results[0].ID.Value == chef1.ID);
        }

        private ChefDao AddChefToDb()
        {
            var chefDao = new ChefDao(Guid.NewGuid(), ValidProperties.ChefName);
            sqlContext.AddAndSave(chefDao);
            return chefDao;
        }
    }
}