using coop2._0.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coop2._0.Model;
using coop2._0.Services;

namespace coop2._0.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            this._transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _transactionService.GetAllTransactions();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransaction(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByUser(int userId)
        {
            return await _transactionService.GetTransactionsByUser(userId);
        }
    

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> RemoveTransaction(int id)
        {
            var deletedTransaction = await _transactionService.RemoveTransaction(id);
            if (deletedTransaction == null)
                return BadRequest();
            return Ok(deletedTransaction);
        }

        [HttpGet("reject/{id:int}")]
        public async Task<ActionResult<Transaction>> RejectTransaction(int id)
        {
            return await _transactionService.RejectTransaction(id);
        }

        [HttpGet("validate/{id:int}")]
        public async Task<ActionResult<Transaction>> ValidateTransaction(int id)
        {
            return await _transactionService.ValidateTransaction(id);
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> AddTransaction(TransactionModel model)
        {
            return await _transactionService.AddTransaction(model);
        }


        [HttpPost]
        [Route("validate-all")]
        public async Task<ActionResult> ValidateAllTransactions(List<int> transactionsIds)
        {
            if (!transactionsIds.Any()) return BadRequest();

            foreach (var transactionId in transactionsIds)
            {
                await _transactionService.ValidateTransaction(transactionId);
            }

            return Ok();
        }

        [HttpPost]
        [Route("reject-all")]
        public async Task<ActionResult> RejectAllTransactions(List<int> transactionsIds)
        {
            if (!transactionsIds.Any()) return BadRequest();

            foreach (var transactionId in transactionsIds)
            {
                await _transactionService.RejectTransaction(transactionId);
            }

            return Ok();
        }

        [HttpPost]
        [Route("remove-all")]
        public async Task<ActionResult> RemoveAllTransactions(List<int> transactionsIds)
        {
            if (!transactionsIds.Any()) return BadRequest();

            foreach (var transactionId in transactionsIds)
            {
                await _transactionService.RemoveTransaction(transactionId);
            }

            return Ok();
        }
    }
}