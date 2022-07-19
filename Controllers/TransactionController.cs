using coop2._0.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using coop2._0.Model;
using coop2._0.Services;

namespace coop2._0.Controllers
{
    [Route("[controller]/{id:int}")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            this._transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _transactionService.GetAllTransactions();
        }

        [HttpGet("")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransaction(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpDelete("")]
        public async Task<ActionResult> RemoveTransaction(int id)
        {
            var deletedTransaction = await _transactionService.RemoveTransaction(id);
            if (deletedTransaction == null)
                return BadRequest();
            return Ok(deletedTransaction);
        }

        [HttpGet("")]
        public async Task<ActionResult<Transaction>> RejectTransaction(int id)
        {
            return await _transactionService.RejectTransaction(id);
        }

        [HttpGet("")]
        public async Task<ActionResult<Transaction>> ValidateTransaction(int id)
        {
            return await _transactionService.ValidateTransaction(id);
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> AddTransaction(TransactionModel model)
        {
            return await _transactionService.AddTransaction(model);
        }
    }
}