using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace SocialChef.Domain.Document
{
    public class RecipeDao : LittleByte.Asp.Database.Document
    {
        public string Name { get; set; }
        public Guid ChefID { get; set; }
        public Guid VariantId { get; set; }
        public ICollection<IngredientDao> Ingredients { get; set; }
        public ICollection<SectionDao> Sections { get; set; }

        [UsedImplicitly]
        public RecipeDao()
        {
            Name = null!;
            Ingredients = null!;
            Sections = null!;
        }

        public RecipeDao(Guid id, Guid chefID, string name, Guid variantId, ICollection<IngredientDao> ingredients, ICollection<SectionDao> sections)
        {
            ID = id;
            ChefID = chefID;
            Name = name;
            Ingredients = ingredients;
            Sections = sections;
            VariantId = variantId;
        }
    }
}