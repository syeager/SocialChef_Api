using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace SocialChef.Domain.Document
{
    public class RecipeDao : LittleByte.Asp.Database.Document
    {
        public string Name { get; set; }
        public Guid ChefID { get; set; }
        public ICollection<SectionDao> Sections { get; set; }

        [UsedImplicitly]
        public RecipeDao()
        {
            Name = null!;
            Sections = null!;
        }

        public RecipeDao(Guid id, Guid chefID, string name, ICollection<SectionDao> sections)
        {
            ID = id;
            ChefID = chefID;
            Name = name;
            Sections = sections;
        }
    }
}