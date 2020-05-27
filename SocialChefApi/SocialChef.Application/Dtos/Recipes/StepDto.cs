namespace SocialChef.Application.Dtos.Recipes
{
    public class StepDto
    {
        public string Instruction { get; set; }

        public StepDto()
        {
            Instruction = null!;
        }

        public StepDto(string instruction)
        {
            Instruction = instruction;
        }
    }
}