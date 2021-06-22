using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProducerService.Models.Request;
using ProducerService.Services;

namespace ProducerService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IAccountService _accountService;

        public AccountsController(ILogger<AccountsController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpPost("OpenAccount")]
        public async Task<IActionResult> OpenAccount(CreateAccountRequest accountRequest)
        {
            return Ok(await _accountService.CreateAccount(accountRequest));
        }

        [HttpPost("Deposit/{accountNumber}")]
        public async Task<IActionResult> Deposit(string accountNumber, UpdateAccountRequest accountRequest)
        {
            try
            {
                await _accountService.Deposit(accountNumber, accountRequest);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("Withdraw/{accountNumber}")]
        public async Task<IActionResult> Withdraw(string accountNumber, UpdateAccountRequest accountRequest)
        {
            try
            {
                await _accountService.Withdraw(accountNumber, accountRequest);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}