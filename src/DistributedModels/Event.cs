using System;

namespace DistributedModels
{
    public class Event
    {
        public string id { get; set; }

        public string deviceId { get; set; }

        public string region { get; set; }

        public int value { get; set; }
    }
}
