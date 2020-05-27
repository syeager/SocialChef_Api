using System.Collections.Generic;

namespace SocialChef.Domain.Recipes
{
    public class Section
    {
        public SectionName Name { get; }
        public IReadOnlyList<Step> Steps { get; }

        // TODO: Make internal.
        public Section(SectionName name, IReadOnlyList<Step> steps)
        {
            Name = name;
            Steps = steps;
        }
    }
}