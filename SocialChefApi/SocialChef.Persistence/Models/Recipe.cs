using JetBrains.Annotations;
using LittleByte.Asp.Database;

namespace SocialChef.Persistence
{
    public class Recipe : Document
    {
        public string Name { get; set; } = null!;

        [UsedImplicitly]
        public Recipe(){}

        public Recipe(string name)
        {
            Name = name;
        }
    }
}