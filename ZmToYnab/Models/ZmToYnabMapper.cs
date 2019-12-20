using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ZmToYnab.Models.YNAB;
using ZmToYnab.Models.ZM;
using ZmToYnab.Services;

namespace ZmToYnab.Models
{
    public class ZmToYnabMapper
    {
        private IMapper MapperIncome { get; set; }
        private IMapper MapperOutcome { get; set; }
        private IMapper MapperTransfer { get; set; }

        public ZmToYnabMapper(ZenMoney zenMoney, Ynab ynab)
        {
            var accountsMapping = zenMoney.GetAccounts().Select(x => new ZmToYnabAccountMapping()
            {
                ZmAccountId = x.Id,
                YnabAccountId = ynab.GetAccounts().First(t => t.Name == x.Title).Id,
                AccountName = x.Title
            }).ToList();
            var categoriesMapping = zenMoney.GetCategories().Select(x => new ZmToYnabCategoriesMapping
            {
                ZmCategoriesId = x.Id,
                YnabCategoriesId = ynab.GetCategories().First(t => t.Name == x.Title).Id,
                CategoryName = x.Title
            }).ToList();

            MapperIncome = CreateIncomeMapper(accountsMapping, categoriesMapping); 
            MapperOutcome = CreateOutcomeMapper(accountsMapping, categoriesMapping);
            MapperTransfer = CreateTransferMapper(accountsMapping, categoriesMapping, ynab.GetPayees());
        }

        private IMapper CreateIncomeMapper(List<ZmToYnabAccountMapping> accountsMapping, List<ZmToYnabCategoriesMapping> categoriesMapping)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ZmTransaction, YnabTransaction>()
                .ForMember(x => x.Amount, opt => opt.MapFrom(s => s.Income * 1000))
                .ForMember(x => x.Cleared, s => s.MapFrom(d => "Cleared"))
                .ForMember(x => x.Approved, s => s.MapFrom(d => true))
                .ForMember(x => x.AccountId, opt => opt.MapFrom(s => accountsMapping.First(t => t.ZmAccountId == s.IncomeAccount).YnabAccountId))
                .ForMember(x => x.CategoryId, opt => opt.MapFrom(s => Guid.Parse("dcb837cf-02b1-4743-af32-d4a9f46f929d")))
                .ForMember(x => x.Memo, opt => opt.MapFrom(s => s.Comment))
                .ForMember(x => x.PayeeName, opt => opt.MapFrom(s => s.Payee))
                .ForMember(x => x.Date, opt => opt.MapFrom(s => s.Date))
                .ForMember(x => x.ImportId, opt => opt.MapFrom(s => s.Id))                
                ;
            });
            return config.CreateMapper();
        }

        private IMapper CreateOutcomeMapper(List<ZmToYnabAccountMapping> accountsMapping, List<ZmToYnabCategoriesMapping> categoriesMapping)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ZmTransaction, YnabTransaction>()
               .ForMember(x => x.Amount, opt => opt.MapFrom(s => s.Outcome * -1000))
               .ForMember(x => x.Cleared, s => s.MapFrom(d => "Cleared"))
               .ForMember(x => x.Approved, s => s.MapFrom(d => true))
               .ForMember(x => x.AccountId, opt => opt.MapFrom(s => accountsMapping.First(t => t.ZmAccountId == s.OutcomeAccount).YnabAccountId))
               .ForMember(x => x.CategoryId, opt => opt.MapFrom(s => categoriesMapping.First(t => t.ZmCategoriesId == s.Tag.First()).YnabCategoriesId))
               .ForMember(x => x.Memo, opt => opt.MapFrom(s => s.Comment))
               .ForMember(x => x.PayeeName, opt => opt.MapFrom(s => s.Payee))
               .ForMember(x => x.Date, opt => opt.MapFrom(s => s.Date))
               .ForMember(x => x.ImportId, opt => opt.MapFrom(s => s.Id))
               ;
            });
            return config.CreateMapper();
        }

        private IMapper CreateTransferMapper(List<ZmToYnabAccountMapping> accountsMapping,
            List<ZmToYnabCategoriesMapping> categoriesMapping, List<YnabPayee> ynabPayees)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ZmTransaction, YnabTransaction>()
                .ForMember(x => x.Amount, opt => opt.MapFrom(s => s.Income * 1000))
                .ForMember(x => x.Cleared, s => s.MapFrom(d => "Cleared"))
                .ForMember(x => x.Approved, s => s.MapFrom(d => true))
                .ForMember(x => x.AccountId, opt => opt.MapFrom(s => accountsMapping.First(t => t.ZmAccountId == s.IncomeAccount).YnabAccountId))
                .ForMember(x => x.CategoryId, opt => opt.MapFrom(s => categoriesMapping.First(t => t.ZmCategoriesId == s.Tag.First()).YnabCategoriesId))
                .ForMember(x => x.Memo, opt => opt.MapFrom(s => s.Comment))
                .ForMember(x => x.Date, opt => opt.MapFrom(s => s.Date))
                .ForMember(x => x.PayeeId, opt => opt.MapFrom(s => ynabPayees.First(x => x.TransferAccountId == accountsMapping.First(t => t.ZmAccountId == s.OutcomeAccount).YnabAccountId).Id))
                .ForMember(x => x.ImportId, opt => opt.MapFrom(s => s.Id))
                .ForMember(x => x.Id, opt => opt.MapFrom(s => s.Id))
                ;
            });
            return config.CreateMapper();
        }

        public List<YnabTransaction> GetIncomeYnabTransaction(List<ZmTransaction> zmTransactions)
        {
            return MapperIncome.Map<List<ZmTransaction>, List<YnabTransaction>>(zmTransactions);
        }

        public List<YnabTransaction> GetOutcomeYnabTransaction(List<ZmTransaction> zmTransactions)
        {
            return MapperOutcome.Map<List<ZmTransaction>, List<YnabTransaction>>(zmTransactions);
        }

        public List<YnabTransaction> GetoTrasferYnabTransaction(List<ZmTransaction> zmTransactions)
        {
            return MapperTransfer.Map<List<ZmTransaction>, List<YnabTransaction>>(zmTransactions);
        }
    }
}
