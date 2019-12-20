using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZmToYnab.Models.YNAB
{
    public struct YnabBudgetData
    {
        [JsonProperty("data")]
        public BudgetData Data { get; set; }
    }

    public struct BudgetData
    {
        [JsonProperty("budgets")]
        public List<YnabBudget> Budgets { get; set; }

        [JsonProperty("default_budget")]
        public object DefaultBudget { get; set; }
    }

    public struct YnabBudget
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("last_modified_on")]
        public DateTimeOffset LastModifiedOn { get; set; }

        [JsonProperty("first_month")]
        public DateTimeOffset FirstMonth { get; set; }

        [JsonProperty("last_month")]
        public DateTimeOffset LastMonth { get; set; }

        [JsonProperty("date_format")]
        public DateFormat DateFormat { get; set; }

        [JsonProperty("currency_format")]
        public CurrencyFormat CurrencyFormat { get; set; }
    }

    public struct CurrencyFormat
    {
        [JsonProperty("iso_code")]
        public string IsoCode { get; set; }

        [JsonProperty("example_format")]
        public string ExampleFormat { get; set; }

        [JsonProperty("decimal_digits")]
        public long DecimalDigits { get; set; }

        [JsonProperty("decimal_separator")]
        public string DecimalSeparator { get; set; }

        [JsonProperty("symbol_first")]
        public bool SymbolFirst { get; set; }

        [JsonProperty("group_separator")]
        public string GroupSeparator { get; set; }

        [JsonProperty("currency_symbol")]
        public string CurrencySymbol { get; set; }

        [JsonProperty("display_symbol")]
        public bool DisplaySymbol { get; set; }
    }

    public struct DateFormat
    {
        [JsonProperty("format")]
        public string Format { get; set; }
    }
}