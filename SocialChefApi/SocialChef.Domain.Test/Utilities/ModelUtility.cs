using NUnit.Framework;

namespace SocialChef.Domain.Test.Utilities
{
    public static class ModelUtility
    {
        public static void AssertFirstError<T>(this ModelConstructResult<T> result, string propertyName, string errorCode) where T : class
        {
            Assert.IsFalse(result.IsSuccess);
            var error = result.Validation.Errors[0];
            Assert.AreEqual(propertyName, error.PropertyName);
            Assert.AreEqual(errorCode, error.ErrorCode);
        }
    }
}