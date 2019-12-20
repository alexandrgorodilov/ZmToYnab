using Newtonsoft.Json;

namespace ZmToYnab.Models.ZM
{
    public struct ZmCompany
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("www")]
        public string Www { get; set; }

        [JsonProperty("country")]
        public long? Country { get; set; }

        [JsonProperty("fullTitle")]
        public string FullTitle { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
    }
}
