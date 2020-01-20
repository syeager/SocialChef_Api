namespace SocialChef.Business.DTOs
{
    public class RecipeDto
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public RecipeDto(string id, string name)
        {
            Name = name;
            ID = id;
        }
    }
}