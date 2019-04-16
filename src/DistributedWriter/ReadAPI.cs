using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using DistributedModels;
using System.Collections.Generic;
using System.Linq;

namespace DistributedAPI
{
    public static class ReadAPI
    {
        [FunctionName("ReadAPI")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "ReadAPI/{deviceId}")] HttpRequest req,
            [CosmosDB(
                databaseName: "eventsDb",
                collectionName: "materialized",
                PreferredLocations = "%REGION%",
                SqlQuery = "SELECT TOP 20 * FROM materialized WHERE materialized.deviceId = {deviceId} ORDER BY materialized._ts DESC",
                ConnectionStringSetting = "CosmosDBAzureFriday"
            )] IEnumerable<MaterializedDevice> materializedViews,
            ILogger log)
        {
            if (materializedViews.Count() == 0)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(materializedViews);
        }
    }
}
