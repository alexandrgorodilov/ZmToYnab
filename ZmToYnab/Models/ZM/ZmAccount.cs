using System;
using Newtonsoft.Json;

namespace ZmToYnab.Models.ZM
{
    public struct ZmAccount
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user")]
        public long User { get; set; }

        [JsonProperty("instrument")]
        public long Instrument { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("role")]
        public long? Role { get; set; }

        [JsonProperty("private")]
        public bool Private { get; set; }

        [JsonProperty("savings")]
        public bool Savings { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("inBalance")]
        public bool InBalance { get; set; }

        [JsonProperty("creditLimit")]
        public long CreditLimit { get; set; }

        [JsonProperty("startBalance")]
        public double StartBalance { get; set; }

        [JsonProperty("balance")]
        public double Balance { get; set; }

        [JsonProperty("company")]
        public long? Company { get; set; }

        [JsonProperty("archive")]
        public bool Archive { get; set; }

        [JsonProperty("enableCorrection")]
        public bool EnableCorrection { get; set; }

        [JsonProperty("startDate")]
        public DateTimeOffset? StartDate { get; set; }

        [JsonProperty("capitalization")]
        public bool? Capitalization { get; set; }

        [JsonProperty("percent")]
        public double? Percent { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }

        [JsonProperty("syncID")]
        public string[] SyncId { get; set; }

        [JsonProperty("enableSMS")]
        public bool EnableSms { get; set; }

        [JsonProperty("endDateOffset")]
        public long? EndDateOffset { get; set; }

        [JsonProperty("endDateOffsetInterval")]
        public string EndDateOffsetInterval { get; set; }

        [JsonProperty("payoffStep")]
        public long? PayoffStep { get; set; }

        [JsonProperty("payoffInterval")]
        public string PayoffInterval { get; set; }
    }
}
