using System.Collections.Generic;
using System.Threading.Tasks;
using coop2._0.Entities;
using Microsoft.AspNetCore.Mvc;

namespace coop2._0.Repositories
{
    public interface ITransactionRepository
    {
        Task<ActionResult<IEnumerable<Transaction>>> GetAllTransactions();
        Task<ActionResult<Transaction>> GetTransaction(int id);
        Task<ActionResult> RemoveTransaction(int id);
        Task<ActionResult<Transaction>> RejectTransaction(int id);
        Task<ActionResult<Transaction>> AddTransaction(double amount, int senderId, int receiverId);
        Task<ActionResult<Transaction>> ValidateTransaction(int id);
    }
}