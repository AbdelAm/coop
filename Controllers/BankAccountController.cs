using coop2._0.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using coop2._0.Entities;

namespace coop2._0.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankService;

        public BankAccountController(IBankAccountService bankService)
        {
            _bankService = bankService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<BankAccount>> GetBankAccountByUser(string userId)
        {
            try
            {
                var bankId = await _bankService.GetBankAccount(userId);
                return Ok(bankId);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}