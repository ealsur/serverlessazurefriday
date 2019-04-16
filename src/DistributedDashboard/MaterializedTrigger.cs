using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DistributedModels;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace DistributedDashboard
{
    public static class MaterializedTrigger
    {
        [FunctionName("Materialized")]
        public static async Task Run(
            [CosmosDBTrigger(
                databaseName: "eventsDb",
                collectionName: "events",
                LeaseCollectionPrefix = "Materialized",
                ConnectionStringSetting = "CosmosDBAzureFriday",
                PreferredLocations = "%REGION%",
                LeaseCollectionName = "leases",
                StartFromBeginning = true
            )] IReadOnlyList<Document> events,
            [CosmosDB(
                databaseName: "eventsDb",
                collectionName: "materialized",
                PreferredLocations = "%REGION%",
                UseMultipleWriteLocations = true,
                ConnectionStringSetting = "CosmosDBAzureFriday"
            )] IAsyncCollector<MaterializedDevice> materializedView,
            ILogger log)
        {
            foreach (var group in events.GroupBy(singleEvent => singleEvent.GetPropertyValue<string>("deviceId")))
            {
                log.LogInformation($"Generating materialized view for device {group.Key}...");
                await materializedView.AddAsync(new MaterializedDevice()
                {
                    deviceId = group.Key,
                    maxValue = group.Max(item => item.GetPropertyValue<int>("value"))
                });
            }
        }
    }
}
