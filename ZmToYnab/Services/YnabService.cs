using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using ZmToYnab.Models.YNAB;

namespace ZmToYnab.Services 
{
    public class YnabService: IYnabService
    {
        private readonly IConfiguration _config;

        public YnabService(IConfiguration config)
        {
            _config = config;
        }

        public YnabBudgetData GetBudgets()
        {
            var restclient = new RestClient(_config["Ynab:Uri:BaseUrl"]);
            restclient.AddDefaultHeader("Authorization", "Bearer " + _config["Ynab:AccessToken"]);
            RestRequest request = new RestRequest("budgets") { Method = Method.GET };
            var response = restclient.Execute(request);
            if (HttpStatusCode.OK != response.StatusCode)
                throw new Exception("Ошибка при получении списка бюджетов" + response.Content);
            var data = response.Content;
            return JsonConvert.DeserializeObject<YnabBudgetData>(data);
        }

        public YnabBudgetData GetBudgets(string accessToken)
        {
            var restclient = new RestClient(_config["Ynab:Uri:BaseUrl"]);
            restclient.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            RestRequest request = new RestRequest("budgets") { Method = Method.GET };
            var response = restclient.Execute(request);
            if (HttpStatusCode.OK != response.StatusCode)
                throw new Exception("Ошибка при получении списка бюджетов" + response.Content);
            var data = response.Content;
            return JsonConvert.DeserializeObject<YnabBudgetData>(data);
        }

        public YnabAccountData GetAccounts(string budgetId)
        {
            var restclient = new RestClient(_config["Ynab:Uri:BaseUrl"]);
            restclient.AddDefaultHeader("Authorization", "Bearer " + _config["Ynab:AccessToken"]);
            RestRequest request = new RestRequest($"/budgets/{budgetId}/accounts") { Method = Method.GET };
            var response = restclient.Execute(request);
            if (HttpStatusCode.OK != response.StatusCode)
                throw new Exception("Ошибка при получении списка счетов" + response.Content);
            var data = response.Content;
            return JsonConvert.DeserializeObject<YnabAccountData>(data);
        }

        public YnabCategoriesData GetCategories(string budgetId)
        {
            var restclient = new RestClient(_config["Ynab:Uri:BaseUrl"]);
            restclient.AddDefaultHeader("Authorization", "Bearer " + _config["Ynab:AccessToken"]);
            RestRequest request = new RestRequest($"/budgets/{budgetId}/categories") { Method = Method.GET };
            var response = restclient.Execute(request);
            if (HttpStatusCode.OK != response.StatusCode)
                throw new Exception("Ошибка при получении списка категорий" + response.Content);
            var data = response.Content;
            return JsonConvert.DeserializeObject<YnabCategoriesData>(data);
        }

        public YnabTransactionsData GetTransactions(string budgetId)
        {
            var restclient = new RestClient(_config["Ynab:Uri:BaseUrl"]);
            restclient.AddDefaultHeader("Authorization", "Bearer " + _config["Ynab:AccessToken"]);
            RestRequest request = new RestRequest($"/budgets/{budgetId}/transactions") { Method = Method.GET };
            var response = restclient.Execute(request);
            if (HttpStatusCode.OK != response.StatusCode)
                throw new Exception("Ошибка при получении списка транзакции" + response.Content);
            var data = response.Content;
            return JsonConvert.DeserializeObject<YnabTransactionsData>(data);
        }

        public YnabPayeeData GetPayees(string budgetId)
        {
            var restclient = new RestClient(_config["Ynab:Uri:BaseUrl"]);
            restclient.AddDefaultHeader("Authorization", "Bearer " + _config["Ynab:AccessToken"]);
            RestRequest request = new RestRequest($"/budgets/{budgetId}/payees") { Method = Method.GET };
            var response = restclient.Execute(request);
            var data = response.Content;
            if (HttpStatusCode.OK != response.StatusCode)
                throw new Exception("Ошибка при получении списка плательщиков" + response.Content);
            return JsonConvert.DeserializeObject<YnabPayeeData>(data);
        }

        public void CreateTransactions(string budgetId, List<YnabTransaction> listTransaction)
        {
            var restclient = new RestClient(_config["Ynab:Uri:BaseUrl"]);
            restclient.AddDefaultHeader("Authorization", "Bearer " + _config["Ynab:AccessToken"]);
            RestRequest request = new RestRequest($"/budgets/{budgetId}/transactions") { Method = Method.POST };

            var listTransactionData = new List<TransactionsData>
            {
                new TransactionsData
                {
                    Transactions = listTransaction
                }
            };

            var str = JsonConvert.SerializeObject(listTransactionData);
            str = str.Remove(0, 1).Remove(str.Length - 2, 1);
            request.AddJsonBody(str);            
            var response = restclient.Execute(request);
            if (HttpStatusCode.Created != response.StatusCode)
                throw new Exception("Ошибка по время создании транзакции" + response.Content);
            var content = JsonConvert.DeserializeObject<YnabTransactionsData>(response.Content);
            if(content.Data.DuplicateImportIds.Count != 0)
                throw new Exception("Попытка загрузить дубли " + string.Join(", ", content.Data.DuplicateImportIds));
        }

        public void CreateTransaction(string budgetId, YnabTransaction transaction)
        {
            var restclient = new RestClient(_config["Ynab:Uri:BaseUrl"]);
            restclient.AddDefaultHeader("Authorization", "Bearer " + _config["Ynab:AccessToken"]);
            RestRequest request = new RestRequest($"/budgets/{budgetId}/transactions/{transaction.Id.ToString()}") { Method = Method.PUT };

            var list = new List<YnabTransaction>();
            list.Add(transaction);
            var listTransactionData = new TransactionData
            {
                Transaction = transaction
            };

            var str = JsonConvert.SerializeObject(listTransactionData);            
            request.AddJsonBody(str);
            var response = restclient.Execute(request);
            if (HttpStatusCode.OK != response.StatusCode)
                throw new Exception("Ошибка по время обновления транзакции" + response.Content);            
        }
    }
}