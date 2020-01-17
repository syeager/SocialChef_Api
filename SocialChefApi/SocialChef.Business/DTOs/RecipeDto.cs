namespace SocialChef.Business.DTOs
{
    public class RecipeDto
    {
        public string Name { get; set; }

        public RecipeDto(string name)
        {
            Name = name;
        }
    }
}