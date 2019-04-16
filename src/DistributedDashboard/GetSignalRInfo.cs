using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;

namespace DistributedDashboard
{
    public static class GetSignalRInfo
    {
        [FunctionName("GetSignalRInfo")]
        public static SignalRConnectionInfo Run(
            [HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
            [SignalRConnectionInfo(HubName = "events", ConnectionStringSetting = "SignalRAzureFriday")] SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }
    }
}
