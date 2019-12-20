using System;
using System.Collections.Generic;
using System.Linq;
using ZmToYnab.Models;
using ZmToYnab.Models.YNAB;
using ZmToYnab.Models.ZM;
using ZmToYnab.Services;

namespace ZmToYnab
{
    public class ConsoleApp
    {
        private readonly ZenMoney _zenMoney;
        private readonly Ynab _ynab;

        public ConsoleApp(IServiceProvider serviceProvider)
        {            
            _zenMoney = new ZenMoney(serviceProvider);
            _ynab = new Ynab(serviceProvider);
        }
        
        public void Run()
        {
            var ynabBudgets = _ynab.GetBudgets();
            for (int i = 0; i < ynabBudgets.Count; i++)
            {
                var ynabBudget = ynabBudgets[i];
                var ynabBudgetId = ynabBudget.Id.ToString();
                
                _ynab.SetParams(ynabBudgetId);
                var result = Enum.TryParse(ynabBudget.CurrencyFormat.IsoCode.ToUpper(), out Currency currency);                
                _zenMoney.SetParams(currency);
                CheckAccounts();
                CheckCategories();
                CheckZenMoneyTransactionOnParentOrNullCategory();
                var transactions = _ynab.GetTransaction();
                if (transactions.Count == 0 && result)
                    FreshStart(ynabBudgetId);
                if (transactions.Count > 0)
                {                    
                    AddNewTransaction(ynabBudgetId);
                    CheckChangeInTransaztion(ynabBudgetId);
                }                    
                CheckAccountBalance(ynabBudgetId);
            }
        }        

        private void FreshStart(string ynabBudgetId)
        {
            var mapper = new ZmToYnabMapper(_zenMoney, _ynab);

            var listTransaction = new List<YnabTransaction>();
            //получаем все транзакции за текущий месяц, чтобы не грузить всё в YNAB
            var startMonthDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //доходные транзакции
            var income = _zenMoney.GetIncomeTransaction()
                .Where(x => x.Date >= startMonthDate)
                .ToList();
            listTransaction.AddRange(mapper.GetIncomeYnabTransaction(income));
            //расходные
            var outcome = _zenMoney.GetOutcomeTransaction()
                .Where(x => x.Date >= startMonthDate)
                .ToList();
            listTransaction.AddRange(mapper.GetOutcomeYnabTransaction(outcome));
            //переводы
            var transfer = _zenMoney.GetTransferTransaction()
                .Where(x => x.Date >= startMonthDate)
                .ToList();
            listTransaction.AddRange(mapper.GetoTrasferYnabTransaction(transfer));
            //текущий баланс
            var balance = _zenMoney.GetBalance()
                .Where(x => x.Date >= startMonthDate)
                .ToList();
            listTransaction.AddRange(mapper.GetIncomeYnabTransaction(balance));
            //переводы в валюте доходные
            var incomeTransfer = _zenMoney.GetTransfersIncomeBetweenAccountsTransactions()
                .Where(x => x.Date >= startMonthDate)
                .ToList();
            listTransaction.AddRange(mapper.GetIncomeYnabTransaction(incomeTransfer));
            //переводы в валюте расходные
            var outconeTransfer = _zenMoney.GetTransfersOutcomeBetweenAccountsTransactions()
                .Where(x => x.Date >= startMonthDate)
                .ToList();
            listTransaction.AddRange(mapper.GetOutcomeYnabTransaction(outconeTransfer));

            CreateNewYnabTransaction(ynabBudgetId, listTransaction);
        }

        private void CreateNewYnabTransaction(string ynabBudgetId, List<YnabTransaction> listTransaction)
        {
            const int countTransaction = 1000;
            var count = listTransaction.Count / countTransaction + 1;
            for (var i = 0; i < count; i++)
            {
                var partTransaction = listTransaction.Skip(i * countTransaction).Take(countTransaction).ToList();
                _ynab.CreateTransactions(ynabBudgetId, partTransaction);
            }
        }
        private void CheckChangeInTransaztion(string ynabBudgetId)
        {
            var zenMoneyTransaction = _zenMoney.GetTransactions();
            var ynabTransaction = _ynab.GetTransaction();

            var changedTransaction = zenMoneyTransaction.Where(x => x.Changed >= Helper.LastClientUnixTimeStamp).ToList();
            if (changedTransaction.Count == 0) return;
            var newZenMoneyTransactions = GetNewZmTransactions();
            var onlyChangedTransaction = changedTransaction.Where(x => !newZenMoneyTransactions.Exists(t => t.Id == x.Id)).ToList();

            var transactions = ynabTransaction.Where(x => onlyChangedTransaction.Exists(t => t.Id.ToString() == x.ImportId)).ToList();

            var mapper = new ZmToYnabMapper(_zenMoney, _ynab);
            var mappingTransaction = new List<YnabTransaction>();
            //расходные
            var outcome = onlyChangedTransaction.Where(x => Math.Abs(x.Income) <= 0).ToList();
            mappingTransaction.AddRange(mapper.GetOutcomeYnabTransaction(outcome).ToList());
            //доходные
            var income = onlyChangedTransaction.Where(x => Math.Abs(x.Outcome) <= 0).ToList();
            mappingTransaction.AddRange(mapper.GetIncomeYnabTransaction(income));
            //переводы
            mappingTransaction.AddRange(mapper.GetoTrasferYnabTransaction(onlyChangedTransaction.Where(x => x.IncomeAccount != x.OutcomeAccount).ToList()));

            for (int i = 0; i < mappingTransaction.Count; i++ )
            {
                var changeYnabTrans = mappingTransaction[i];
                var ynabTrans = transactions.Where(x => x.ImportId == changeYnabTrans.ImportId).FirstOrDefault();
                if (changeYnabTrans.AccountId != ynabTrans.AccountId)
                    ynabTrans.AccountId = changeYnabTrans.AccountId;
                if (changeYnabTrans.Amount != ynabTrans.Amount)
                    ynabTrans.Amount = changeYnabTrans.Amount;
                if (changeYnabTrans.CategoryId != ynabTrans.CategoryId)
                    ynabTrans.CategoryId = changeYnabTrans.CategoryId;
                if (changeYnabTrans.Date != ynabTrans.Date)
                    ynabTrans.Date = changeYnabTrans.Date;
                if (changeYnabTrans.Memo != ynabTrans.Memo)
                    ynabTrans.Memo = changeYnabTrans.Memo;
                if (changeYnabTrans.TransferAccountId != ynabTrans.TransferAccountId)
                    ynabTrans.TransferAccountId = changeYnabTrans.TransferAccountId;
                if (changeYnabTrans.PayeeName != ynabTrans.PayeeName)
                    ynabTrans.PayeeName = changeYnabTrans.PayeeName;

                _ynab.CreateTransaction(ynabBudgetId, ynabTrans);
            }
        }

        private void AddNewTransaction(string ynabBudgetId)
        {
            var newZenMoneyTransactions = GetNewZmTransactions();
            if (newZenMoneyTransactions.Count == 0) return;

            var mapper = new ZmToYnabMapper(_zenMoney, _ynab);

            var mappingTransaction = new List<YnabTransaction>();
            //доходные транзакции
            var income = _zenMoney.GetIncomeTransaction()
                .ToList();
            mappingTransaction.AddRange(mapper.GetIncomeYnabTransaction(income));
            //расходные
            var outcome = _zenMoney.GetOutcomeTransaction()                
                .ToList();
            mappingTransaction.AddRange(mapper.GetOutcomeYnabTransaction(outcome));
            //переводы
            var transfer = _zenMoney.GetTransferTransaction()                
                .ToList();
            mappingTransaction.AddRange(mapper.GetoTrasferYnabTransaction(transfer));
            //переводы в валюте доходные
            var incomeTransfer = _zenMoney.GetTransfersIncomeBetweenAccountsTransactions()                
                .ToList();
            mappingTransaction.AddRange(mapper.GetIncomeYnabTransaction(incomeTransfer));
            //переводы в валюте расходные
            var outconeTransfer = _zenMoney.GetTransfersOutcomeBetweenAccountsTransactions()
                .ToList();
            mappingTransaction.AddRange(mapper.GetOutcomeYnabTransaction(outconeTransfer));

            mappingTransaction = mappingTransaction
                                    .Where(x => newZenMoneyTransactions.Exists(t => t.Id.ToString() == x.ImportId))
                                    .ToList();
            CreateNewYnabTransaction(ynabBudgetId, mappingTransaction);
        }

        private List<ZmTransaction> GetNewZmTransactions()
        {
            var ynabTransaction = _ynab.GetTransaction();
            var zenMoneyTransaction = _zenMoney.GetTransactions();

            return zenMoneyTransaction
                .Where(x => !ynabTransaction.Exists(t => t.ImportId == x.Id.ToString()))
                .ToList();
        }

        private void CheckAccounts()
        {
            var ynabAccounts = _ynab.GetAccounts().Select(x => x.Name).ToList();
            var zenMoneyAccounts = _zenMoney.GetAccounts().Select(x => x.Title).ToList();
            var except = zenMoneyAccounts.Except(ynabAccounts).ToList();
            var countExcept = except.Count;
            var exceptAccount = string.Join(", ", except);
            if (countExcept != 0)
                throw new Exception("В Дзен-мани появились новые счета " + exceptAccount);
        }

        private void CheckCategories()
        {
            var ynabCategories = _ynab.GetCategories().Select(x => x.Name).ToList();
            var zenMoneyCategories = _zenMoney.GetCategories().Select(x => x.Title).ToList();
            var except = zenMoneyCategories.Except(ynabCategories).ToList();
            var countExcept = except.Count;
            var exceptAccount = string.Join(", ", except);
            if (countExcept != 0)
                throw new Exception("В Дзен-мани появились новые категории " + exceptAccount);
        }

        private void CheckZenMoneyTransactionOnParentOrNullCategory()
        {
            var zenMoneyIncomeOutcomeTransaction = _zenMoney.GetTransactions()
                .Where(x => x.IncomeAccount == x.OutcomeAccount)
                .ToList();
            var zenMoneyNullCategories = zenMoneyIncomeOutcomeTransaction
                .Where(x => x.Tag == null)
                .ToList();
            if (zenMoneyNullCategories.Count != 0)
                throw new Exception("В Дзен-мани появились транзакции без категорий ");
            var zenMoneyParentCategories = _zenMoney.GetAllCategories().Where(x => x.Parent == null).ToList();
            var zenMoneyTransaction = zenMoneyIncomeOutcomeTransaction
                .Where(x => x.Tag != null) //исключаем транзакции без категорий
                .Where( x => zenMoneyParentCategories.Exists(t => t.Id == x.Tag.FirstOrDefault()))
                .ToList();
            if (zenMoneyTransaction.Count != 0)
                throw new Exception("В Дзен-мани появились транзакции c родительской категорией");
        }

        private void CheckAccountBalance(string ynabBudgetId)
        {
            _ynab.SetParams(ynabBudgetId);
            var ynabAccounts = _ynab.GetAccounts();
            var zenMoneyAccounts = _zenMoney.GetAccounts()
                .Select(x => new YnabAccount { Balance = Convert.ToInt64(x.Balance * 1000), Name = x.Title})
                .ToList();
            var except = zenMoneyAccounts.Where(x => !ynabAccounts.Exists(t => t.Name == x.Name && t.Balance == x.Balance)).Select(x => x.Name).ToList();
            var exceptCount = except.Count;
            var exceptAccunts = string.Join(", ", except);
            if (exceptCount != 0)
                throw new Exception("Расхождение в балансе по следующим аккаунтам " + exceptAccunts);
        }
    }        
}
