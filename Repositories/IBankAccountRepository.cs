using coop2._0.Entities;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public interface IBankAccountRepository
    {
        Task<string> InsertBankAccount(BankAccount account);
    }
}
