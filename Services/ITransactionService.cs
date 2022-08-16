using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public interface ITransactionService
    {
        Task<object> GetAllTransactions(PaginationFilter filter);
        Task<Transaction> GetTransaction(int id);
        Task<ActionResult> RemoveTransaction(int id);
        Task<ActionResult<Transaction>> RejectTransaction(int id);
        Task<ActionResult> AddTransaction(Transaction model);
        Task<ActionResult<Transaction>> ValidateTransaction(int id);
        Task<object> GetTransactionsByUser(int userBankAccountId, PaginationFilter filter);
        Task<object> SearchForTransactions(string keyword, PaginationFilter filter);
        Task<IEnumerable<Transaction>> GetAllTransactions();
        Task<IEnumerable<Transaction>> GetTransactionsByUser(int userBankAccountId);
    }
}