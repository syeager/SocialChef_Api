using System;
using System.Threading.Tasks;
using LittleByte.Asp.Test.Database;
using LittleByte.Asp.Test.Utilities;
using LittleByte.Domain.Test.Utilities;
using NUnit.Framework;
using SocialChef.Business.Relational;
using SocialChef.Domain.Chefs;
using SocialChef.Domain.Recipes;

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
            var chefId = new Chef.Guid(Guid.Empty);

            AssertExtension.ThrowsNotFoundAsync<Chef>(() => testObj.FindByIdAsync(chefId), chefId.Value);
        }

        [Test]
        public async Task FindByIdAsync_HasChef_ReturnChef()
        {
            var chefDao = AddChefToDb();

            var chef = await testObj.FindByIdAsync(new Chef.Guid(chefDao.ID));

            Assert.AreEqual(chefDao.ID, chef.ID.Value);
        }

        [Test]
        public void FindByUserAsync_NoChef_ThrowNotFound()
        {
            var userId = new User.Guid(Guid.Empty);

            AssertExtension.ThrowsNotFoundAsync<User>(() => testObj.FindByUserAsync(userId), userId.Value);
        }

        [Test]
        public async Task FindByUserAsync_HasChef_ReturnChef()
        {
            var chefDao = AddChefToDb();

            var chef = await testObj.FindByUserAsync(new User.Guid(chefDao.UserID));

            Assert.AreEqual(chefDao.ID, chef.ID.Value);
        }

        private ChefDao AddChefToDb()
        {
            var chefDao = new ChefDao(Guid.NewGuid(), ValidModelValues.ChefName);
            sqlContext.AddAndSave(chefDao);
            return chefDao;
        }
    }
}