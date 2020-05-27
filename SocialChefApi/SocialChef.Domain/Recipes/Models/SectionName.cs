namespace SocialChef.Domain.Recipes
{
    public readonly struct SectionName
    {
        internal string Value { get; }

        // TODO: Make internal.
        public SectionName(string value) => Value = value.Trim();

        public override string ToString() => Value;

        public static implicit operator string(SectionName name) => name.Value;
    }
}