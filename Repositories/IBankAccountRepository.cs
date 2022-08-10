using coop2._0.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public interface IBankAccountRepository
    {
        Task<string> InsertBankAccount(BankAccount account);
        Task<BankAccount> SelectByUser(string userId);
        Task Delete(BankAccount account);
    }
}
