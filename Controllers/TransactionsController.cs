using coop2._0.Entities;
using coop2._0.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coop2._0.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging.EventSource;

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
        [Authorize(Roles = "ADMIN")]
        public async Task<object> GetTransactions([FromQuery] PaginationFilter filter)
        {
            return await _transactionService.GetAllTransactions(filter);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransaction(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpGet("user/{userBankAccountId:int}")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<object> GetTransactionsByUser([FromQuery] PaginationFilter filter
            , int userBankAccountId)
        {
            return await _transactionService.GetTransactionsByUser(userBankAccountId, filter);
        }


        [HttpDelete("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> RemoveTransaction(int id)
        {
            return await _transactionService.RemoveTransaction(id);
            ;
        }

        [HttpGet("reject/{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Transaction>> RejectTransaction(int id)
        {
            return await _transactionService.RejectTransaction(id);
        }

        [HttpGet("validate/{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Transaction>> ValidateTransaction(int id)
        {
            return await _transactionService.ValidateTransaction(id);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<ActionResult<Transaction>> AddTransaction(Transaction model)
        {
            return await _transactionService.AddTransaction(model);
        }


        [HttpPost]
        [Route("validate-all")]
        [Authorize(Roles = "ADMIN")]
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
        [Authorize(Roles = "ADMIN")]
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
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> RemoveAllTransactions(List<int> transactionsIds)
        {
            if (!transactionsIds.Any()) return BadRequest();

            foreach (var transactionId in transactionsIds)
            {
                await _transactionService.RemoveTransaction(transactionId);
            }

            return Ok();
        }

        [HttpGet("search/{keyword}")]
        [Authorize(Roles = "ADMIN,USER")]
        public async Task<object> SearchForTransactions([FromQuery] PaginationFilter filter, string keyword)
        {
            return await _transactionService.SearchForTransactions(keyword, filter);
        }
    }
}