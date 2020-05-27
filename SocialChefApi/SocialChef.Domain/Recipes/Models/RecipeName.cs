namespace SocialChef.Domain.Recipes
{
    public readonly struct RecipeName
    {
        internal string Value { get; }

        internal RecipeName(string value) => Value = value.Trim();

        public override string ToString() => Value;

        public static implicit operator string(RecipeName recipeName) => recipeName.Value;
    }
}