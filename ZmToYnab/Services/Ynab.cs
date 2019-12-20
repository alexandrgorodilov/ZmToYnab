using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ZmToYnab.Models.YNAB;

namespace ZmToYnab.Services
{
    public class Ynab
    {
        private readonly IYnabService _ynabService;
        private List<YnabAccount> YnabAccounts { get; set; }
        private List<YnabCategory> YnabCategories { get; set; }
        private List<YnabPayee> YnabPayees { get; set; }
        private List<YnabBudget> YnabBudgets { get; set; }
        private List<YnabTransaction> YnabTransactions { get; set; }
        
        public Ynab(IServiceProvider serviceProvider)
        {
            _ynabService = serviceProvider.GetService<IYnabService>();
            YnabBudgets = _ynabService.GetBudgets().Data.Budgets.Where(x => !x.Name.Contains("Archived")).ToList();
        }

        public void SetParams(string ynabBudgetId)
        {
            YnabAccounts = _ynabService.GetAccounts(ynabBudgetId).Data.Accounts;
            YnabCategories = _ynabService.GetCategories(ynabBudgetId).Data.CategoryGroups.SelectMany(x => x.Categories).ToList();
            YnabTransactions = _ynabService.GetTransactions(ynabBudgetId).Data.Transactions;
            YnabPayees = _ynabService.GetPayees(ynabBudgetId).Data.Payees;
        }

        public List<YnabBudget> GetBudgets()
        {
            return YnabBudgets;
        }

        public List<YnabTransaction> GetTransaction()
        {
            return YnabTransactions;
        }

        public List<YnabAccount> GetAccounts()
        {
            return YnabAccounts;
        }

        public List<YnabPayee> GetPayees()
        {
            return YnabPayees;
        }

        public List<YnabCategory> GetCategories()
        {
            return YnabCategories;
        }

        public void CreateTransactions(string ynabBudgetId, List<YnabTransaction> listTransactions)
        {
            _ynabService.CreateTransactions(ynabBudgetId, listTransactions);
        }

        public void CreateTransaction(string budgetId, YnabTransaction transaction)
        {
            _ynabService.CreateTransaction(budgetId, transaction);
        }


    }
}