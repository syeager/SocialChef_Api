using LittleByte.Asp.Business;

namespace SocialChef.Business.DTOs
{
    public class RecipeDto : Dto
    {
        public string Name { get; set; }

        public RecipeDto(string id, string name)
        {
            Name = name;
            ID = id;
        }
    }
}