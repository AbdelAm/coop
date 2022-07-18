using coop2._0.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }


        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<ActionResult> RemoveTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null || transaction.Status == Status.Approved) return null;

            _context.Transactions.Remove(transaction);

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<ActionResult<Transaction>> RejectTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction is not { Status: Status.Progress }) return null;
            transaction.Status = Status.Rejected;
            _context.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<ActionResult<Transaction>> AddTransaction(double amount, int senderId, int receiverId)
        {
            var sender = await _context.BankAccounts.FindAsync(senderId);
            var receiver = await _context.BankAccounts.FindAsync(receiverId);

            if (sender == null || receiver == null) return null;

            var transaction = new Transaction()
            {
                SenderBankAccountId = senderId,
                ReceiverBankAccountId = receiverId,
                SenderBankAccount = sender,
                ReceiverBankAccount = receiver,
                Amount = amount,
                DateTransaction = DateTime.Now
            };
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<ActionResult<Transaction>> ValidateTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction is not { Status: Status.Progress }) return null;

            transaction.SenderBankAccount.Balance -= transaction.Amount;
            transaction.ReceiverBankAccount.Balance += transaction.Amount;
            transaction.Status = Status.Approved;
            _context.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
    }
}