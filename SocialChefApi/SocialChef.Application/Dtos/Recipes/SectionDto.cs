using System.Collections.Generic;

namespace SocialChef.Application.Dtos.Recipes
{
    public class SectionDto
    {
        public string Name { get; set; }
        public List<StepDto> Steps { get; set; }

        public SectionDto()
        {
            Name = null!;
            Steps = null!;
        }

        public SectionDto(string name, List<StepDto> steps)
        {
            Name = name;
            Steps = steps;
        }
    }
}