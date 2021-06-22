using System;
using System.Threading.Tasks;
using ProducerService.Models;
using ProducerService.Models.Request;
using ProducerService.Repositories;

namespace ProducerService.Services
{
    public interface IAccountService
    {
        Task<Account> CreateAccount(CreateAccountRequest accountRequest);
        Task Deposit(string accountNumber, UpdateAccountRequest accountRequest);
        Task Withdraw(string accountNumber, UpdateAccountRequest accountRequest);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IConsumerService _consumerSerivce;

        public AccountService(IAccountRepository accountRepository, IConsumerService consumerService)
        {
            _accountRepository = accountRepository;
            _consumerSerivce = consumerService;
        }

        public Task<Account> CreateAccount(CreateAccountRequest accountRequest)
        {
            var account = new Account();
            account.TransactionNumber = Guid.NewGuid().ToString();
            account.Number = Guid.NewGuid().ToString();
            account.Name = accountRequest.AccountName;
            account.Amount = accountRequest.Amount;
            account.Action = ActionEnum.OPEN_ACCOUNT.ToString();
            account.UpdateLog = DateTime.Now;

            return _accountRepository.CreateAccount(account);
        }

        public async Task Deposit(string accountNumber, UpdateAccountRequest accountRequest)
        {
            try
            {
                var request = _consumerSerivce.GetAccount(accountNumber);
                var account = new Account();
                account.TransactionNumber = Guid.NewGuid().ToString();
                account.Number = request.Result.Number;
                account.Name = request.Result.Name;
                account.Amount = accountRequest.Amount;
                account.Action = ActionEnum.DEPOSIT.ToString();
                account.UpdateLog = DateTime.Now;

                await _accountRepository.Deposit(account);
            }
            catch (Exception)
            {
                throw new Exception("Cannot find your account with this account number!");
            }
        }

        public async Task Withdraw(string accountNumber, UpdateAccountRequest accountRequest)
        {
            try
            {
                var request = _consumerSerivce.GetAccount(accountNumber);
                var account = new Account();
                account.TransactionNumber = Guid.NewGuid().ToString();
                account.Number = request.Result.Number;
                account.Name = request.Result.Name;
                account.Amount = accountRequest.Amount;
                account.Action = ActionEnum.WITHDRAW.ToString();
                account.UpdateLog = DateTime.Now;

                await _accountRepository.Deposit(account);
            }
            catch (Exception)
            {
                throw new Exception("Cannot find your account with this account number!");
            }
        }
    }
}