using System;
using System.Threading.Tasks;
using ProducerService.Models;
using ProducerService.Models.Request;
using ProducerService.Repositories;
using ProducerService.Services.Producer;
using System.Text.Json;

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
        private readonly KafkaProducerService _producer;

        public AccountService(IAccountRepository accountRepository, IConsumerService consumerService, KafkaProducerService producer)
        {
            _accountRepository = accountRepository;
            _consumerSerivce = consumerService;
            _producer = producer;
        }

        public async Task<Account> CreateAccount(CreateAccountRequest accountRequest)
        {
            var account = new Account();
            account.TransactionNumber = Guid.NewGuid().ToString();
            account.Number = Guid.NewGuid().ToString();
            account.Name = accountRequest.AccountName;
            account.Amount = accountRequest.Amount;
            account.Action = ActionEnum.OPEN_ACCOUNT.ToString();
            account.UpdateLog = DateTime.Now;

            await _accountRepository.CreateAccount(account);
            _producer.produceToKakfa(JsonSerializer.Serialize(account));
            return account;
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
                _producer.produceToKakfa(JsonSerializer.Serialize(account));
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
                _producer.produceToKakfa(JsonSerializer.Serialize(account));
            }
            catch (Exception)
            {
                throw new Exception("Cannot find your account with this account number!");
            }
        }
    }
}