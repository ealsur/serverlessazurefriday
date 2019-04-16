using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DistributedModels;

namespace DistributedAPI
{
    public static class WriteEvent
    {
        [FunctionName("WriteEvent")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] Event eventData,
            [CosmosDB(
                databaseName: "eventsDb",
                collectionName: "events",
                PreferredLocations = "%REGION%",
                UseMultipleWriteLocations = true,
                ConnectionStringSetting = "CosmosDBAzureFriday"
            )] IAsyncCollector<Event> eventCollector,
            ILogger log)
        {
            // Validating
            if (eventData == null)
            {
                return new BadRequestResult();
            }

            eventData.region = Environment.GetEnvironmentVariable("REGION");
            log.LogInformation($"Received event on {eventData.region} for device {eventData.deviceId}");

            // Saving
            await eventCollector.AddAsync(eventData);

            return new OkResult();
        }
    }
}
