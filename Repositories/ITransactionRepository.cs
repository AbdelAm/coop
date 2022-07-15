using System.Collections.Generic;
using System.Threading.Tasks;
using coop2._0.Entities;
using Microsoft.AspNetCore.Mvc;

namespace coop2._0.Repositories
{
    public interface ITransactionRepository
    {
        Task<ActionResult<IEnumerable<Transaction>>> GetTransactions();
        Task<ActionResult<Transaction>> GetTransaction(int id);
    }
}