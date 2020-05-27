using NUnit.Framework;
using SocialChef.Domain.Recipes;

namespace SocialChef.Domain.Test.Domains.Recipes.Models
{
    public class StepTest
    {
        [Test]
        public void Ctor_InstructionWhitespace_Trimmed()
        {
            var step = new Step(" a ");

            Assert.AreEqual("a", step.Instruction);
        }
    }
}