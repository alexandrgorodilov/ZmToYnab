using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ZmToYnab.Models;
using ZmToYnab.Models.ZM;

namespace ZmToYnab.Services
{
    public class ZenMoney
    {
        public ZmDiff ZmDiff { get; }
        private List<ZmAccount> ZmAccounts { get; set; }
        private List<ZmTag> ZmCategories { get; set; }
        private List<ZmTransaction> Transactions { get; set; }
        private List<ZmTransaction> BalanceTransaction { get; set; }
        private List<ZmTransaction> IncomeZmTransaction { get; set; }
        private List<ZmTransaction> OutcomeZmTransaction { get; set; }
        private List<ZmTransaction> TransferZmTransaction { get; set; }
        private List<ZmTransaction> TransfersOutcomeBetweenAccounts { get; set; }
        private List<ZmTransaction> TransfersIncomeBetweenAccounts { get; set; }

        public ZenMoney(IServiceProvider serviceProvider)
        {
            var zenMoneyService = serviceProvider.GetService<IZenMoneyService>();
            ZmDiff = zenMoneyService.GetDiff();
        }

        public void SetParams(Currency currency)
        {
            SetZenMoneyAccountsInCurrency(currency);
            SetCategories();
            SetTransactionsInCurrency(currency);
            var zmTransactions = ZmDiff.Transaction.Where(x => !x.Deleted).ToList();
            var zmTransactionsInCurrency = GetTransactions();

            //переводы между валютными счетами
            var transfersBetweenBudgets = zmTransactions.Except(zmTransactions.Except(zmTransactions.Except(zmTransactionsInCurrency))).ToList();
            var transfersBetweenBudgetsIncomeNotCurrency = transfersBetweenBudgets.Where(x => (Currency)x.IncomeInstrument != currency).ToList();
            var transfersBetweenBudgetsOutcomeNotCurrency = transfersBetweenBudgets.Where(x => (Currency)x.OutcomeInstrument != currency).ToList();
            
            TransfersOutcomeBetweenAccounts = transfersBetweenBudgetsIncomeNotCurrency.Except(transfersBetweenBudgetsOutcomeNotCurrency).ToList();
            TransfersIncomeBetweenAccounts = transfersBetweenBudgets.Except(transfersBetweenBudgetsIncomeNotCurrency).ToList();

            IncomeZmTransaction = zmTransactionsInCurrency.Where(x => Math.Abs(x.Outcome) <= 0).ToList();
            OutcomeZmTransaction = zmTransactionsInCurrency.Where(x => Math.Abs(x.Income) <= 0).ToList();
            TransferZmTransaction = zmTransactionsInCurrency.Where(x => x.IncomeAccount != x.OutcomeAccount).ToList();

            //текущие балансы в виде транзакции
            BalanceTransaction = ZmAccounts
                .Where(x => Math.Abs(x.StartBalance) > 0)
                .Select(x => new ZmTransaction
                {
                    Income = x.Balance,
                    Tag = new List<Guid> { Guid.Parse("dcb837cf-02b1-4743-af32-d4a9f46f929d") },
                    IncomeAccount = x.Id,
                    Date = new DateTimeOffset(DateTime.Now.Year - 5, DateTime.Now.Month + 1, 1, 0, 0, 0, 0, new TimeSpan(5, 0, 0)),
                    Id = Guid.NewGuid()
                }).ToList();
        }
        
        public List<ZmAccount> GetAccounts()
        {
            return ZmAccounts;
        }
        
        private void SetZenMoneyAccountsInCurrency(Currency currency)
        {
            ZmAccounts = ZmDiff.Account.Where(x => (Currency)x.Instrument == currency).ToList();
        }

        public List<ZmTag> GetCategories()
        {
            return ZmCategories;
        }

        private void SetCategories()
        {
            ZmCategories = GetAllCategories()
                .Where(x => x.Parent != null && //исключаем родительские категории
                            x.Parent != Guid.Parse("f6aaf324-9647-4b36-992b-454f51be7c8a") && //исключаем подкатегории Тегов
                            x.Parent != Guid.Parse("313f7741-b7c5-4b8a-970e-67dd428aeca8") //исключаем подкатегории Доходов
                ).ToList();
        }
        public List<ZmTag> GetAllCategories()
        {
            return ZmDiff.Tag;
        }

        public List<ZmTransaction> GetTransactions()
        {
            return Transactions;
        }
        
        private void SetTransactionsInCurrency(Currency currency)
        {
            Transactions =  ZmDiff.Transaction
                .Where(x => !x.Deleted)
                .Where(x =>(Currency) x.IncomeInstrument == currency && (Currency) x.OutcomeInstrument == currency)
                .ToList();
        }

        public List<ZmTransaction> GetBalance()
        {
            return BalanceTransaction;
        }

        public List<ZmTransaction> GetIncomeTransaction()
        {
            return IncomeZmTransaction;
        }
        
        public List<ZmTransaction> GetOutcomeTransaction()
        {
            return OutcomeZmTransaction;
        }
        
        public List<ZmTransaction> GetTransferTransaction()
        {
            return TransferZmTransaction;
        }

        public List<ZmTransaction> GetTransfersOutcomeBetweenAccountsTransactions()
        {
            return TransfersOutcomeBetweenAccounts;
        }

        public List<ZmTransaction> GetTransfersIncomeBetweenAccountsTransactions()
        {
            return TransfersIncomeBetweenAccounts;
        }
    }
}
