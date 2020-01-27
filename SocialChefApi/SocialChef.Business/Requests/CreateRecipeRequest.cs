using JetBrains.Annotations;

namespace SocialChef.Business.Requests
{
    public class CreateRecipeRequest
    {
        public string Name { get; set; } = null!;

        [UsedImplicitly]
        public CreateRecipeRequest()
        {
        }

        public CreateRecipeRequest(string name)
        {
            Name = name;
        }
    }
}