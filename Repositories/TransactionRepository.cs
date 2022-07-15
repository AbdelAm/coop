using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using coop2._0.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace coop2._0.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }


       
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            return await _context.Transactions.FindAsync(id);;
        }
    }
}