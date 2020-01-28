using JetBrains.Annotations;
using LittleByte.Asp.Database;

namespace SocialChef.Persistence
{
    public class Recipe : Document
    {
        public string Name { get; [UsedImplicitly]set; }

        [UsedImplicitly]
        public Recipe()
        {
            Name = null!;
        }

        public Recipe(string name)
        {
            Name = name;
        }
    }
}