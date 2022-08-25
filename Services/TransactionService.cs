using System.Collections.Generic;
using coop2._0.Entities;
using coop2._0.Model;
using coop2._0.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }


        public async Task<object> GetAllTransactions(PaginationFilter filter)
        {
            return await _transactionRepository.GetAllTransactions(filter);
        }

        public async Task<Transaction> GetTransaction(int id)
        {
            return await _transactionRepository.GetTransaction(id);
        }

        public Task<ActionResult> RemoveTransaction(int id)
        {
            return _transactionRepository.RemoveTransaction(id);
        }

        public async Task<ActionResult<Transaction>> RejectTransaction(int id)
        {
            return await _transactionRepository.RejectTransaction(id);
        }

        public async Task<ActionResult> AddTransaction(TransactionModel transactionModel)
        {
            return await _transactionRepository.AddTransaction(transactionModel);
        }

        public async Task<ActionResult<Transaction>> ValidateTransaction(int id)
        {
            return await _transactionRepository.ValidateTransaction(id);
        }

        public async Task<object> GetTransactionsByUser(int userBankAccountId, PaginationFilter filter)
        {
            return await _transactionRepository.GetTransactionsByUser(userBankAccountId, filter);
        }

        public async Task<object> SearchForTransactions(string keyword, PaginationFilter filter)
        {
            return await _transactionRepository.SearchForTransactions(keyword, filter);
        }

        public Task<IEnumerable<TransactionResponse>> GetAllTransactions()
        {
            return _transactionRepository.GetAllTransactions();
        }

        public Task<IEnumerable<TransactionResponse>> GetTransactionsByUser(int userBankAccountId)
        {
            return _transactionRepository.GetTransactionsByUser(userBankAccountId);
        }


        public Task<object> GetAllTransactionsByStatus(Status status, PaginationFilter filter)
        {
            return _transactionRepository.GetAllTransactionsByStatus(status,filter);
        }
    }
}