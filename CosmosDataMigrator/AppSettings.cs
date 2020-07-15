namespace CosmosDataMigrator
{
    public class AppSettings
    {
        public string ContainerName { get; set; } = string.Empty;

        public string DatabaseName { get; set; } = string.Empty;

        public string SourceConnectionString { get; set; } = string.Empty;

        public string DestinationConnectionString { get; set; } = string.Empty;

        public int MaxItemCount { get; set; }

        public int MaxRetryAttemptsOnRateLimitedRequests { get; set; }

        public int MaxRetryWaitTimeOnRateLimitedRequestsInSeconds { get; set; }

        public override string ToString()
        {
            return string.Format(
                "Database Name:\t {0}\n" +
                "Container Name:\t {1}\n\n" +
                "Max Item Count:\t\t\t {4}\n" +
                "Max Retry Attmpts Count:\t {5}\n" +
                "Max Retry Wait Time:\t\t {6}(s)\n\n" +
                "Source Connection String:\n\n {2}\n\n" +
                "Destination Connection String:\n\n {3}\n",                
                DatabaseName, ContainerName, SourceConnectionString, DestinationConnectionString, MaxItemCount, MaxRetryAttemptsOnRateLimitedRequests, MaxRetryWaitTimeOnRateLimitedRequestsInSeconds);
        }
    }
}
