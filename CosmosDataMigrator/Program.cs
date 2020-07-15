using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosDataMigrator
{
    class Program
    {
        static async Task Main()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Async(a => a.Console())
                .WriteTo.Async(a => a.File($"log-{DateTime.Now.ToFileTimeUtc()}.txt"))
                .CreateLogger();

            var config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .AddJsonFile("appsettings.json", false, false)
                .Build();

            AppSettings appSettings = new AppSettings();
            config.Bind(appSettings);

            Console.WriteLine(string.Format($"##### DATA MIGRATION #####\n\n{appSettings}\n\nPress any key to continue..."));
            Console.ReadLine();

            var cosmosClientOptions = new CosmosClientOptions
            {
                MaxRetryAttemptsOnRateLimitedRequests = appSettings.MaxRetryAttemptsOnRateLimitedRequests,
                MaxRetryWaitTimeOnRateLimitedRequests = TimeSpan.FromSeconds(appSettings.MaxRetryWaitTimeOnRateLimitedRequests)
            };

            using var sourceClient = new CosmosClient(appSettings.SourceConnectionString);
            var sourceContainer = sourceClient.GetContainer(appSettings.DatabaseName, appSettings.ContainerName);

            var documentsToMigrate = new List<dynamic>();

            try
            {
                using FeedIterator<dynamic> resultSet = sourceContainer.GetItemQueryIterator<dynamic>(
                    queryDefinition: null,
                    requestOptions: new QueryRequestOptions
                    {
                        MaxItemCount = appSettings.MaxItemCount                        
                    });

                while (resultSet.HasMoreResults)
                {
                    FeedResponse<dynamic> response = await resultSet.ReadNextAsync();

                    var itemList = response.Resource.ToList();

                    itemList.ForEach(item => Log.Information($"Retrieved document with id: {item.id}"));

                    documentsToMigrate.AddRange(itemList);
                }
            } 
            catch (Exception ex)
            {
                Log.Error($"An error occurred while retrieving documents from source: {ex.GetType().Name}, {ex.Message}");
                return;
            }

            Log.Information($"{documentsToMigrate.Count} retrieved from source, press any key to proceed with write operation...");
            Console.ReadLine();

            List<Task> concurrentTasks = new List<Task>();
            
            using var destinationClient = new CosmosClient(appSettings.DestinationConnectionString);
            var destinationContainer = destinationClient.GetContainer(appSettings.DatabaseName, appSettings.ContainerName);

            foreach (var itemToInsert in documentsToMigrate)
                concurrentTasks.Add(destinationContainer.CreateItemAsync(itemToInsert));

            try
            {
                await Task.WhenAll(concurrentTasks);
                Log.Information($"{documentsToMigrate.Count} documents were successfully written to the destination database.");
            }
            catch (Exception ex)
            {
                Log.Error($"Unable to write all documents to destination. {ex.GetType().Name}: {ex.Message}");
            }

            Log.CloseAndFlush();
        }
    }
}
