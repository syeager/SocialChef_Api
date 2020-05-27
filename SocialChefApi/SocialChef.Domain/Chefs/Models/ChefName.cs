namespace SocialChef.Domain.Chefs
{
    public readonly struct ChefName
    {
        internal string Value { get; }

        internal ChefName(string value) => Value = value.Trim();

        public override string ToString() => Value;

        public static implicit operator string(ChefName chefName) => chefName.Value;
    }
}