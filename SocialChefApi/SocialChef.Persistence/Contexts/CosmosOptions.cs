namespace SocialChef.Persistence
{
    public class CosmosOptions
    {
        public const string OptionsKey = "CosmosDB";

        public string Url { get; set; }
        public string Key { get; set; }
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }

        public CosmosOptions()
        {
            Url = "";
            Key = "";
            DatabaseName = "";
            ContainerName = "";
        }
    }
}