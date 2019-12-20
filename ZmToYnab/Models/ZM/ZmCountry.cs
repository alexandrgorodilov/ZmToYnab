using Newtonsoft.Json;

namespace ZmToYnab.Models.ZM
{
    public struct ZmCountry
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("currency")]
        public long Currency { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }
    }
}
