namespace SocialChef.Persistence
{
    public class CosmosOptions
    {
        public const string OptionsKey = "CosmosDB";

        public string Url { get; set; } = null!;
        public static string Key { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}