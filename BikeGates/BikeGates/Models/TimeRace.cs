using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikeGates.Models
{
    public class TimeRace
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "isFinished")]
        public int isFinished { get; set; }

        [JsonProperty(PropertyName = "totalTime")]
        public int TotalTime { get; set; }

        public DateTimeOffset timestamp { get; set; }
    }
}
