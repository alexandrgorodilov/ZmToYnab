using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZmToYnab.Models.ZM
{
    public struct ZmDiff
    {
        [JsonProperty("instrument")]
        public List<ZmInstrument> Instrument { get; set; }

        [JsonProperty("country")]
        public List<ZmCountry> Country { get; set; }

        [JsonProperty("company")]
        public List<ZmCompany> Company { get; set; }

        [JsonProperty("user")]
        public List<ZmUser> User { get; set; }

        [JsonProperty("account")]
        public List<ZmAccount> Account { get; set; }

        [JsonProperty("tag")]
        public List<ZmTag> Tag { get; set; }

        [JsonProperty("budget")]
        public List<ZmBudget> Budget { get; set; }

        [JsonProperty("merchant")]
        public List<ZmMerchant> Merchant { get; set; }        

        [JsonProperty("transaction")]
        public List<ZmTransaction> Transaction { get; set; }
    }
}
