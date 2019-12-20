using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace ZmToYnab.Models.YNAB
{

    public struct YnabTransactionsData
    {
        [JsonProperty("data")]
        public TransactionsData Data { get; set; }
    }

    public struct TransactionsData
    {
        [JsonProperty("transactions")]
        public List<YnabTransaction> Transactions { get; set; }

        [JsonProperty("duplicate_import_ids")]
        public List<string> DuplicateImportIds { get; set; }
    }

    public struct TransactionData
    {
        [JsonProperty("transaction")]
        public YnabTransaction Transaction { get; set; }
    }

    public struct YnabTransaction
    {
        [JsonProperty("account_id")]
        public Guid AccountId { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("payee_id")]
        public Guid? PayeeId { get; set; }

        [JsonProperty("payee_name")]
        public string PayeeName { get; set; }

        [JsonProperty("category_id")]
        public Guid? CategoryId { get; set; }

        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("cleared")]
        public string Cleared { get; set; }

        [JsonProperty("approved")]
        public bool Approved { get; set; }

        [JsonProperty("transfer_account_id")]
        public Guid? TransferAccountId { get; set; }

        [JsonProperty("import_id")]
        public string ImportId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("flag_color")]
        public string FlagColor { get; set; }

        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("category_name")]
        public string CategoryName { get; set; }

        [JsonProperty("transfer_transaction_id")]
        public string TransferTransactionId { get; set; }

        [JsonProperty("matched_transaction_id")]
        public object MatchedTransactionId { get; set; }

        [JsonProperty("deleted")]
        public bool Deleted { get; set; }

        [JsonProperty("subtransactions")]
        public object[] Subtransactions { get; set; }
    
    }
}
