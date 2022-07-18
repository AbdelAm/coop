using System.Collections.Generic;
using System.Threading.Tasks;
using coop2._0.Entities;
using coop2._0.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace coop2._0.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }


        public async Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions()
        {
            return await _transactionRepository.GetAllTransactions();
        }

        public async Task<ActionResult<Transaction>> GetTransaction(int id)
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

        public async Task<ActionResult<Transaction>> AddTransaction(double amount, int senderId, int receiverId)
        {
            return await _transactionRepository.AddTransaction(amount, senderId, receiverId);
        }

        public async Task<ActionResult<Transaction>> ValidateTransaction(int id)
        {
            return await _transactionRepository.ValidateTransaction(id);
        }

        
    }
}