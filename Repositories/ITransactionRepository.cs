using coop2._0.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using coop2._0.Model;

namespace coop2._0.Repositories
{
    public interface ITransactionRepository
    {
        Task<object> GetAllTransactions(PaginationFilter filter);
        Task<Transaction> GetTransaction(int id);
        Task<ActionResult> RemoveTransaction(int id);
        Task<ActionResult<Transaction>> RejectTransaction(int id);
        Task<ActionResult<Transaction>> AddTransaction(Transaction transaction);
        Task<ActionResult<Transaction>> ValidateTransaction(int id);
        Task<object> GetTransactionsByUser(int userId, PaginationFilter filter);
        Task<IEnumerable<Transaction>> SearchForTransactions(string keyword);
    }
}