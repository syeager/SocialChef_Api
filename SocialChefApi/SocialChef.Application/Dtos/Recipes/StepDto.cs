namespace SocialChef.Application.Dtos
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