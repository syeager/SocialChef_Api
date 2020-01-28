using JetBrains.Annotations;

namespace SocialChef.Business.Requests
{
    public class CreateRecipeRequest
    {
        public string Name { get; [UsedImplicitly]set; }

        [UsedImplicitly]
        public CreateRecipeRequest()
        {
            Name = null!;
        }

        public CreateRecipeRequest(string name)
        {
            Name = name;
        }
    }
}