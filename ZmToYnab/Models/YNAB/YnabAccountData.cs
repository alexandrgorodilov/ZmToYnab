using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZmToYnab.Models.YNAB
{
    public struct YnabAccountData
    {
        [JsonProperty("data")]
        public AccountData Data { get; set; }
    }

    public struct AccountData
    {
        [JsonProperty("accounts")]
        public List<YnabAccount> Accounts { get; set; }
    }

    public struct YnabAccount
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("on_budget")]
        public bool OnBudget { get; set; }

        [JsonProperty("closed")]
        public bool Closed { get; set; }

        [JsonProperty("note")]
        public object Note { get; set; }

        [JsonProperty("balance")]
        public long Balance { get; set; }

        [JsonProperty("cleared_balance")]
        public long ClearedBalance { get; set; }

        [JsonProperty("uncleared_balance")]
        public long UnclearedBalance { get; set; }

        [JsonProperty("transfer_payee_id")]
        public Guid TransferPayeeId { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }
    }
}
