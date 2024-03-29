﻿using coop2._0.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public async Task<IEnumerable<BankAccount>> SelectByUser(string userId)
        {
            return await _context.BankAccounts.Where(b => b.UserId == userId)
                .ToListAsync();
        }
        public async Task<bool> HasActivated(string userId)
        {
            var bankAccounts =  await _context.BankAccounts.Where(b => b.UserId == userId && b.Status == Status.Approuved && b.Balance > 0)
                .CountAsync();
            return bankAccounts > 0;
        }
        public async Task Delete(BankAccount account)
        {
            _context.BankAccounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }
}