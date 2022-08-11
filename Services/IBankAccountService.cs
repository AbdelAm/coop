using System.Threading.Tasks;
using coop2._0.Entities;

namespace coop2._0.Services
{
    public interface IBankAccountService
    {
        Task<BankAccount> GetBankAccount(string userId);
    }
}
