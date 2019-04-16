using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DistributedDashboard
{
    public static class NotificationsTrigger
    {
        [FunctionName("Notifications")]
        public static async Task Run(
            [CosmosDBTrigger(
                databaseName: "eventsDb",
                collectionName: "events",
                LeaseCollectionPrefix = "Notifications",
                ConnectionStringSetting = "CosmosDBAzureFriday",
                PreferredLocations = "%REGION%",
                LeaseCollectionName = "leases")]
                IReadOnlyList<Document> events,
            [SignalR(HubName = "events", ConnectionStringSetting = "SignalRAzureFriday")] 
                IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            await signalRMessages.AddAsync(new SignalRMessage()
            {
                Target = "console",
                Arguments = new[] {
                    events.Select(singleEvent => JsonConvert.DeserializeObject<ConsoleLog>(singleEvent.ToString()))
                }
            });
        }
    }
}
