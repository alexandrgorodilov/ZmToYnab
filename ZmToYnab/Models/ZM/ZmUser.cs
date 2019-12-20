using Newtonsoft.Json;

namespace ZmToYnab.Models.ZM
{
    public struct ZmUser
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("country")]
        public long Country { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }

        [JsonProperty("currency")]
        public long Currency { get; set; }

        [JsonProperty("paidTill")]
        public long PaidTill { get; set; }

        [JsonProperty("parent")]
        public long? Parent { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("subscription")]
        public string Subscription { get; set; }
    }
}
