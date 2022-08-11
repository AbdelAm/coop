﻿using coop2._0.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        public async Task<BankAccount> SelectByUser(string userId)
        {
            return await _context.BankAccounts.FirstOrDefaultAsync(b => b.UserId == userId && b.Status == Status.Approuved);
        }
    }
}
