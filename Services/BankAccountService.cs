using coop2._0.Entities;
using coop2._0.Repositories;
using System;
using System.Linq;
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

        public async Task<BankAccount> GetBankAccount(string userId)
        {
            var bankAccounts = await _bankRepository.SelectByUser(userId);

            var enumerable = bankAccounts.ToList();

            if (!enumerable.Any())
            {
                throw new Exception("There is no Bank Account with this information");
            }

            var bankAccount = enumerable.FirstOrDefault();

            return bankAccount;
        }
    }
}