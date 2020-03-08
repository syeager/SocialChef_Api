namespace SocialChef.Persistence.Models
{
    public class RecipeStep
    {
        public string Instruction { get; set; }

        public RecipeStep(string instruction)
        {
            Instruction = instruction;
        }
    }
}