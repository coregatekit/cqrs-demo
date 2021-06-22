using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerService.Models;
using ConsumerService.Models.Request;

namespace ConsumerService.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAccounts();
        Task<Account> GetAccount(string accountNumber);
        Task<Account> CreateAccount(CreateAccountRequest requestAccount);
        Task UpdateAccount(UpdateAccountRequest accountRequest);
    }

    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository acccountRepository)
        {
            _accountRepository = acccountRepository;
        }

        public async Task<Account> CreateAccount(CreateAccountRequest accountRequest)
        {
            var account = new Account();
            account.Number = accountRequest.AccountNumber;
            account.Name = accountRequest.AccountName;
            account.Amount = accountRequest.Amount;
            account.UpdateDate = accountRequest.UpdateDate;
            return await _accountRepository.CreateAccount(account);
        }

        public async Task<Account> GetAccount(string accountNumber)
        {
            return await _accountRepository.GetAccount(accountNumber);
        }

        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await _accountRepository.GetAccounts();
        }

        public async Task UpdateAccount(UpdateAccountRequest accountRequest)
        {
            var account = await _accountRepository.GetAccount(accountRequest.AccountNumber);
            account.UpdateDate = account.UpdateDate;
            if (accountRequest.Action == ActionEnum.DEPOSIT.ToString())
            {
                account.Amount += accountRequest.Amount;
            }
            if (accountRequest.Action == ActionEnum.WITHDRAW.ToString())
            {
                if (account.Amount < accountRequest.Amount)
                { 
                    throw new Exception("Cannot withdraw your money is lessthan request");
                }
                account.Amount -= accountRequest.Amount;
            }
            await _accountRepository.UpdateAccount(accountRequest.AccountNumber, account);
        }
    }
}