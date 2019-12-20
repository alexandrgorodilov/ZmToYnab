using System;
using Newtonsoft.Json;

namespace ZmToYnab.Models.ZM
{
    public struct ZmTag
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user")]
        public long User { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("budgetIncome")]
        public bool BudgetIncome { get; set; }

        [JsonProperty("budgetOutcome")]
        public bool BudgetOutcome { get; set; }

        [JsonProperty("required")]
        public bool TagRequired { get; set; }

        [JsonProperty("color")]
        public long? Color { get; set; }

        [JsonProperty("picture")]
        public object Picture { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("showIncome")]
        public bool ShowIncome { get; set; }

        [JsonProperty("showOutcome")]
        public bool ShowOutcome { get; set; }

        [JsonProperty("parent")]
        public Guid? Parent { get; set; }
    }
}
