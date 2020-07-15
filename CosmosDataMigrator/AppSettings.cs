namespace CosmosDataMigrator
{
    public class AppSettings
    {
        public string ContainerName { get; set; } = string.Empty;

        public string DatabaseName { get; set; } = string.Empty;

        public string SourceConnectionString { get; set; } = string.Empty;

        public string DestinationConnectionString { get; set; } = string.Empty;

        public int MaxItemCount { get; set; }

        public override string ToString()
        {
            return string.Format(
                "Database Name:\t {0}\n" +
                "Container Name:\t {1}\n" +
                "Max Item Count:\t {4}\n\n" +
                "Source Connection String:\t {2}\n\n" +
                "Destination Connection String: {3}",                
                DatabaseName, ContainerName, SourceConnectionString, DestinationConnectionString, MaxItemCount);
        }
    }
}
