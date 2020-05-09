using System.Collections.Generic;
using JetBrains.Annotations;

namespace SocialChef.Business.Document
{
    public class SectionDao
    {
        public string Name { get; set; }
        public ICollection<StepDao> Steps { get; set; }

        [UsedImplicitly]
        public SectionDao()
        {
            Name = null!;
            Steps = null!;
        }

        public SectionDao(string name, ICollection<StepDao> steps)
        {
            Name = name;
            Steps = steps;
        }
    }
}