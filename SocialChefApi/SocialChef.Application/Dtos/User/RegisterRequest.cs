using JetBrains.Annotations;

namespace SocialChef.Application.Dtos.User
{
    public class RegisterRequest
    {
        public string Email { get; [UsedImplicitly] set; }
        public string Password { get; [UsedImplicitly] set; }

        [UsedImplicitly]
        public RegisterRequest()
        {
            Email = null!;
            Password = null!;
        }

        public RegisterRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}