using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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


        public async Task<Transaction> GetTransaction(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<ActionResult> RemoveTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null || transaction.Status == Status.Approuved) return new BadRequestResult();

            _context.Transactions.Remove(transaction);

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<ActionResult<Transaction>> RejectTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction is not { Status: Status.Progress }) return new BadRequestResult();

            transaction.Status = Status.Rejected;

            _context.Entry(transaction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<ActionResult> AddTransaction(Transaction transaction)
        {
            if (transaction == null) return new BadRequestResult();

            transaction.DateTransaction = DateTime.Now;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<ActionResult<Transaction>> ValidateTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction is not { Status: Status.Progress }) return new BadRequestResult();
            BankAccount senderBankAccount = null;
            BankAccount receiverBankAccount = null;


            senderBankAccount = await _context.BankAccounts.FindAsync(transaction.SenderBankAccountId);
            receiverBankAccount = await _context.BankAccounts.FindAsync(transaction.ReceiverBankAccountId);


            if (senderBankAccount == null || receiverBankAccount == null)
                return null;

            senderBankAccount.Balance -= transaction.Amount;
            receiverBankAccount.Balance += transaction.Amount;

            transaction.Status = Status.Approuved;

            _context.Entry(transaction).State = EntityState.Modified;
            _context.Entry(senderBankAccount).State = EntityState.Modified;
            _context.Entry(receiverBankAccount).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<object> GetAllTransactions(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var response = await _context.Transactions.Include(t => t.SenderBankAccount.User)
                .Include(t => t.ReceiverBankAccount.User)
                .OrderBy(d => d.DateTransaction)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToListAsync();

            var pagination = new PaginationResponse(validFilter.PageNumber, validFilter.PageSize,
                await _context.Transactions.CountAsync());


            return new { response, pagination };
        }

        public async Task<object> SearchForTransactions(string keyword, PaginationFilter filter)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return null;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var response = await _context.Transactions.Where(t =>
                    (t.SenderBankAccount.User.Name.ToLower().Contains(keyword.Trim().ToLower())) ||
                    (t.ReceiverBankAccount.User.Name.ToLower().Contains(keyword.Trim().ToLower())) ||
                    (t.Motif.ToLower().Contains(keyword.Trim().ToLower()))
                )
                .Include(t => t.SenderBankAccount.User)
                .Include(t => t.ReceiverBankAccount.User)
                .OrderBy(t => t.DateTransaction)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToListAsync();

            var totalRecords = await _context.Transactions.CountAsync(t =>
                (t.SenderBankAccount.User.Name.ToLower().Contains(keyword.Trim().ToLower())) ||
                (t.ReceiverBankAccount.User.Name.ToLower().Contains(keyword.Trim().ToLower())) ||
                (t.Motif.ToLower().Contains(keyword.Trim().ToLower())));

            var pagination = new PaginationResponse(validFilter.PageNumber, validFilter.PageSize,
                totalRecords);
            return new
            {
                response,
                pagination
            };
        }

        public async Task<object> GetTransactionsByUser(int userBankAccountId, PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var response =
                await _context.Transactions
                    .Where(b => b.ReceiverBankAccountId == userBankAccountId ||
                                b.SenderBankAccountId == userBankAccountId)
                    .Include(b => b.ReceiverBankAccount.User)
                    .OrderBy(d => d.DateTransaction)
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();
            var totalRecords = await _context.Transactions.CountAsync(b =>
                b.ReceiverBankAccountId == userBankAccountId ||
                b.SenderBankAccountId == userBankAccountId);

            var pagination = new PaginationResponse(validFilter.PageNumber, validFilter.PageSize,
                totalRecords);

            return new { response, pagination };
        }
    }
}