using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public interface ITransactionRepository
    {
        Task<object> GetAllTransactions(PaginationFilter filter);
        Task<Transaction> GetTransaction(int id);
        Task<ActionResult> RemoveTransaction(int id);
        Task<ActionResult<Transaction>> RejectTransaction(int id);
        Task<ActionResult> AddTransaction(TransactionModel transactionModel);
        Task<ActionResult<Transaction>> ValidateTransaction(int id);
        Task<object> GetTransactionsByUser(int userBankAccountId, PaginationFilter filter);
        Task<object> SearchForTransactions(string keyword, PaginationFilter filter);

        Task<IEnumerable<TransactionResponse>> GetAllTransactions();
        Task<IEnumerable<TransactionResponse>> GetTransactionsByUser(int userBankAccountId);
        Task<object> GetAllTransactionsByStatus(Status status, PaginationFilter filter);
    }
}