using Newtonsoft.Json;

namespace ZmToYnab.Models.ZM
{
    public struct ZmInstrument
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("shortTitle")]
        public string ShortTitle { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("rate")]
        public double Rate { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }
    }
}
