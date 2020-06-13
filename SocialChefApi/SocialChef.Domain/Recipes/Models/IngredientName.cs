namespace SocialChef.Domain.Recipes
{
    public readonly struct IngredientName
    {
        internal string Value { get; }

        public IngredientName(string value) => Value = value.Trim();

        public override string ToString() => Value;

        public static implicit operator string(IngredientName name) => name.Value;
    }
}