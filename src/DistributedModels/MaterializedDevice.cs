using System;

namespace DistributedModels
{
    public class MaterializedDevice
    {
        public string id { get; set; } = DateTime.UtcNow.Ticks.ToString();

        public string deviceId { get; set; }

        public int maxValue { get; set; }
    }
}
