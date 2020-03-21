using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialChef.Business.Document.Models
{
    public class Recipe : LittleByte.Asp.Database.Document
    {
        public string Name { get; set; }
        public Guid ChefID { get; set; }
        public ICollection<RecipeStep> Steps { get; set; }

        public Recipe()
        {
            Name = null!;
            Steps = null!;
        }

        public Recipe(Guid chefID, string name, IEnumerable<string> steps)
        {
            ChefID = chefID;
            Name = name;
            Steps = steps.Select(s => new RecipeStep(s)).ToArray();
        }
    }
}