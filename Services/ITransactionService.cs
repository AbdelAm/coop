using System.Collections.Generic;
using System.Threading.Tasks;
using coop2._0.Entities;
using Microsoft.AspNetCore.Mvc;

namespace coop2._0.Services
{
    public interface ITransactionService
    {
        Task<ActionResult<Transaction>> GetTransaction(int id);
        Task<ActionResult<IEnumerable<Transaction>>> GetTransactions(int id);
    }
}