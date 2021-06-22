using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerService.Models;
using ConsumerService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ConsumerService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IAccountService _accountService;

        public AccountsController(ILogger<AccountsController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _accountService.GetAccounts());
        }

        [HttpGet("{accountNumber}")]
        public async Task<IActionResult> GetAccount(string accountNumber)
        {
            var account = await _accountService.GetAccount(accountNumber);
            if (account == null)
            {
                return NotFound();
            }
            return Ok(account);
        }
    }
}
