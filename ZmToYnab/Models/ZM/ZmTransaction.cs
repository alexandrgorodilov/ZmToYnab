using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZmToYnab.Models.ZM
{
    public struct ZmTransaction
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user")]
        public long User { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("income")]
        public double Income { get; set; }

        [JsonProperty("outcome")]
        public double Outcome { get; set; }

        [JsonProperty("changed")]
        public long Changed { get; set; }

        [JsonProperty("incomeInstrument")]
        public long IncomeInstrument { get; set; }

        [JsonProperty("outcomeInstrument")]
        public long OutcomeInstrument { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("originalPayee")]
        public string OriginalPayee { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("viewed")]
        public bool Viewed { get; set; }

        [JsonProperty("hold")]
        public bool? Hold { get; set; }

        [JsonProperty("qrCode")]
        public string QrCode { get; set; }

        [JsonProperty("incomeAccount")]
        public Guid IncomeAccount { get; set; }

        [JsonProperty("outcomeAccount")]
        public Guid OutcomeAccount { get; set; }

        [JsonProperty("tag")]
        public List<Guid> Tag { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("payee")]
        public string Payee { get; set; }

        [JsonProperty("opIncome")]
        public double? OpIncome { get; set; }

        [JsonProperty("opOutcome")]
        public double? OpOutcome { get; set; }

        [JsonProperty("opIncomeInstrument")]
        public long? OpIncomeInstrument { get; set; }

        [JsonProperty("opOutcomeInstrument")]
        public long? OpOutcomeInstrument { get; set; }

        [JsonProperty("latitude")]
        public double? Latitude { get; set; }

        [JsonProperty("longitude")]
        public double? Longitude { get; set; }

        [JsonProperty("merchant")]
        public Guid? Merchant { get; set; }

        [JsonProperty("incomeBankID")]
        public string IncomeBankId { get; set; }

        [JsonProperty("outcomeBankID")]
        public string OutcomeBankId { get; set; }

        [JsonProperty("reminderMarker")]
        public Guid? ReminderMarker { get; set; }
    }
}
