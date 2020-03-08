using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace SocialChef.Business.Requests
{
    public class CreateRecipeRequest
    {
        public string Name { get; [UsedImplicitly]set; }
        public Guid AuthorID { get; set; }
        public ICollection<string> Steps { get; [UsedImplicitly]set; }

        [UsedImplicitly]
        public CreateRecipeRequest()
        {
            Name = null!;
            Steps = null!;
        }

        public CreateRecipeRequest(string name, ICollection<string> steps)
        {
            Name = name;
            Steps = steps;
        }
    }
}