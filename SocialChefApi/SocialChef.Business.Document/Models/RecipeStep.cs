﻿namespace SocialChef.Business.Document.Models
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