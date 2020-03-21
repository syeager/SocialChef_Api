using System;
using JetBrains.Annotations;

namespace SocialChef.Identity.Transport
{
    public class UserDto
    {
        public Guid ID { get; [UsedImplicitly] set; }

        [UsedImplicitly]
        public UserDto() {}

        public UserDto(Guid id)
        {
            ID = id;
        }
    }
}