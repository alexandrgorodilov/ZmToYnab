using System.Collections.Generic;
using ZmToYnab.Models.YNAB;

namespace ZmToYnab.Services
{
    public interface IYnabService
    {
        YnabBudgetData GetBudgets();

        YnabBudgetData GetBudgets(string accessToken);

        YnabAccountData GetAccounts(string budgetId);

        YnabCategoriesData GetCategories(string budgetId);

        YnabTransactionsData GetTransactions(string budgetId);

        void CreateTransactions(string budgetId, List<YnabTransaction> listTransaction);

        YnabPayeeData GetPayees(string budgetId);

        void CreateTransaction(string budgetId, YnabTransaction transaction);
    }
}