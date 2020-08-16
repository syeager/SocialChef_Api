using JetBrains.Annotations;

namespace SocialChef.Application.Dtos
{
    public class CreateChefDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        [UsedImplicitly]
        public CreateChefDto()
        {
            Name = null!;
            Email = null!;
            Password = null!;
            PasswordConfirm = null!;
        }

        public CreateChefDto(string name, string email, string password, string passwordConfirm)
        {
            Name = name;
            Email = email;
            Password = password;
            PasswordConfirm = passwordConfirm;
        }
    }
}