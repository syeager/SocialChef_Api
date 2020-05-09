using JetBrains.Annotations;

namespace SocialChef.Business.Document
{
    public class StepDao
    {
        public int Index { get; set; }
        public string Instruction { get; set; }

        [UsedImplicitly]
        public StepDao()
        {
            Instruction = null!;
        }

        public StepDao(int index, string instruction)
        {
            Instruction = instruction;
            Index = index;
        }
    }
}