using System.Threading.Tasks;
using IdentityServer4.Stores;

namespace SocialChef.Identity.Quickstart
{
    public static class ClientStoreExtension
    {
        /// <summary>
        /// Determines whether the client is configured to use PKCE.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="clientID">The client identifier.</param>
        /// <returns></returns>
        public static async Task<bool> IsPkceClientAsync(this IClientStore store, string clientID)
        {
            if(string.IsNullOrWhiteSpace(clientID))
            {
                return false;
            }

            var client = await store.FindEnabledClientByIdAsync(clientID);
            return client?.RequirePkce == true;
        }
    }
}