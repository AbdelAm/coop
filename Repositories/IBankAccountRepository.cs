using coop2._0.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public interface IBankAccountRepository
    {
        Task<string> InsertBankAccount(BankAccount account);
        Task<IEnumerable<BankAccount>> SelectByUser(string userId);
        Task<bool> HasActivated(string userId);
        Task Delete(BankAccount account);
    }
}