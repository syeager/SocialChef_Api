namespace SocialChef.Persistence
{
    public class CosmosOptions
    {
        public const string OptionsKey = "CosmosDB";

        public string Url { get; set; } = null!;
        public string Key { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string ContainerName { get; set; } = null!;
    }
}