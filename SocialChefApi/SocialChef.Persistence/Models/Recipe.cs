using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using LittleByte.Asp.Database;

namespace SocialChef.Persistence
{
    public class Recipe : Document
    {
        public string Name { get; [UsedImplicitly]set; }
        public ICollection<RecipeStep> Steps { get; [UsedImplicitly]set; }

        [UsedImplicitly]
        public Recipe()
        {
            Name = null!;
            Steps = null!;
        }

        public Recipe(string name, IEnumerable<string> steps)
        {
            Name = name;
            Steps = steps.Select(s => new RecipeStep(s)).ToArray();
        }
    }
}