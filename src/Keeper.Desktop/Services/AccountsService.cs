using Keeper.Desktop.Models;
using Microsoft.EntityFrameworkCore;

namespace Keeper.Desktop.Services
{
    public class AccountsService : ModelsService<Account>
    {
        public AccountsService(DatabaseContext _context) : base(_context) { }

        public override DbSet<Account> GetDbSet(DatabaseContext context) => context.Accounts;

        public int UpdateBalance(Account account, decimal amount)
        {
            account.Balance += amount;
            context.Accounts.Update(account);
            return context.SaveChanges();
        }
    }
}
