namespace SocialChef.Domain.Recipes
{
    public class Step
    {
        // TODO: Make StepInstruction.
        public string Instruction { get; }

        // TODO: Make internal.
        public Step(string instruction)
        {
            Instruction = instruction.Trim();
        }
    }
}