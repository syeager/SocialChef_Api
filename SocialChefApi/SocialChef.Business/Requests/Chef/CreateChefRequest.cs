using JetBrains.Annotations;

namespace SocialChef.Business.Requests
{
    public class CreateChefRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        [UsedImplicitly]
        public CreateChefRequest()
        {
            Name = null!;
            Email = null!;
            Password = null!;
            PasswordConfirm = null!;
        }

        public CreateChefRequest(string name, string email, string password, string passwordConfirm)
        {
            Name = name;
            Email = email;
            Password = password;
            PasswordConfirm = passwordConfirm;
        }
    }
}