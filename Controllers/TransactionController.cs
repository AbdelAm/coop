using coop2._0.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using coop2._0.Services;

namespace coop2._0.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            this._transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _transactionService.GetTransactions();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction =await _transactionService.GetTransaction(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }
    }
}