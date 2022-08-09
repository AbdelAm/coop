using System.Threading.Tasks;

namespace coop2._0.Services
{
    public interface IBankAccountService
    {
        Task<int> GetBankAccount(string userId);
    }
}
