using System;
using Newtonsoft.Json;

namespace ZmToYnab.Models.ZM
{
    public struct ZmMerchant
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user")]
        public long User { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }
    }
}
