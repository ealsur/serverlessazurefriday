using System;
using System.Globalization;

namespace DistributedDashboard
{
    public class ConsoleLog
    {
        public string region { get; set; }

        public string deviceId { get; set; }

        public int value { get; set; }

        public string time { get; set; } = DateTime.UtcNow.ToString("r", CultureInfo.InvariantCulture);
    }
}
