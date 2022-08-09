using coop2._0.Repositories;
using System;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankRepository;

        public BankAccountService(IBankAccountRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public async Task<int> GetBankAccount(string userId)
        {
            var bank = await _bankRepository.SelectByUser(userId);
            if (bank == null)
            {
                throw new Exception("There is no Bank Account with this information");
            }
            return bank.Id;
        }
    }
}
