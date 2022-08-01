using coop2._0.Entities;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public BankAccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> InsertBankAccount(BankAccount account)
        {
            _context.BankAccounts.Add(account);
            await _context.SaveChangesAsync();

            return account.AccountNumber;
        }
    }
}
