using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZmToYnab.Models.YNAB
{
    public struct YnabPayeeData
    {
        [JsonProperty("data")]
        public PayeeData Data { get; set; }
    }

    public struct PayeeData
    {
        [JsonProperty("payees")]
        public List<YnabPayee> Payees { get; set; }
    }

    public struct YnabPayee
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("transfer_account_id")]
        public Guid? TransferAccountId { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }
    }
}
