using System.Collections.Generic;
using OpenIddict.Abstractions;

namespace SocialChef.Domain.Identity.Config
{
    public static class Scopes
    {
        public static IEnumerable<OpenIddictScopeDescriptor> Get()
        {
            return new[]
            {
                new OpenIddictScopeDescriptor
                {
                    Name = "demo_api",
                }
            };
        }
    }
}