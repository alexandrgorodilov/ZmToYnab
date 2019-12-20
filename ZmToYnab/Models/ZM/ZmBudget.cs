using System;
using Newtonsoft.Json;

namespace ZmToYnab.Models.ZM
{
    public struct ZmBudget
    {
        [JsonProperty("user")]
        public long User { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("tag")]
        public Guid? Tag { get; set; }

        [JsonProperty("income")]
        public long Income { get; set; }

        [JsonProperty("outcome")]
        public long Outcome { get; set; }

        [JsonProperty("incomeLock")]
        public bool IncomeLock { get; set; }

        [JsonProperty("outcomeLock")]
        public bool OutcomeLock { get; set; }
    }
}
