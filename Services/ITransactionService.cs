using System.Collections.Generic;
using System.Threading.Tasks;
using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Mvc;

namespace coop2._0.Services
{
    public interface ITransactionService
    {
        Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions();
        Task<ActionResult<Transaction>> GetTransaction(int id);
        Task<ActionResult> RemoveTransaction(int id);
        Task<ActionResult<Transaction>> RejectTransaction(int id);
        Task<ActionResult<Transaction>> AddTransaction(TransactionModel model);
        Task<ActionResult<Transaction>> ValidateTransaction(int id);
    }
}