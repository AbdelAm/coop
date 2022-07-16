using System.Collections.Generic;
using System.Threading.Tasks;
using coop2._0.Entities;
using coop2._0.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace coop2._0.Services
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }


        public Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            return _transactionRepository.GetTransaction(id);
        }

        public Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return _transactionRepository.GetTransactions();
        }
    }
}