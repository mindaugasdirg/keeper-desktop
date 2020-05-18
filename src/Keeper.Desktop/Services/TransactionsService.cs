using Keeper.Desktop.Models;
using Keeper.Desktop.Models.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace Keeper.Desktop.Services
{
    public class TransactionsService : ModelsService<Transaction>
    {
        private AccountsService accountsService;
        public TransactionsService(DatabaseContext _context, AccountsService _accountsService)
            : base(_context)
        {
            accountsService = _accountsService;
        }

        public override DbSet<Transaction> GetDbSet(DatabaseContext context) => context.Transactions;

        public List<Transaction> GetAccountTransactions(Account account) =>
            context.Transactions.Include(t => t.Account).Include(t => t.Category).Where(t => t.Account.Id == account.Id).ToList();

        public override int HandleTransaction(DataTransaction.ActionType type, Transaction obj)
        {
            switch(type)
            {
                case DataTransaction.ActionType.Create:
                    accountsService.UpdateBalance(obj.Account, obj.Amount);
                    break;
                case DataTransaction.ActionType.Edit:
                    var transaction = context.Transactions.Where(t => t.Id == obj.Id).Single();
                    accountsService.UpdateBalance(obj.Account, transaction.Amount - obj.Amount);
                    break;
                case DataTransaction.ActionType.Delete:
                    accountsService.UpdateBalance(obj.Account, -obj.Amount);
                    break;
            }
            return base.HandleTransaction(type, obj);
        }

        public List<SpendingByDay> GetSpendByDay(Account account)
        {
            var today = DateTime.Now;
            var groups = context.Transactions
                .Where(t => account == t.Account && today.Day - t.Date.Day < 14)
                .AsEnumerable()
                .GroupBy(t => t.Date.Day)
                .ToList();

            return groups.Select(g => new SpendingByDay()
                {
                    Day = g.Key,
                    Sum = g.Sum(t => t.Amount)
                })
                .ToList();
        }
    }
}
