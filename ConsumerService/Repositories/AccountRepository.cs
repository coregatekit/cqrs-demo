using System.Collections.Generic;
using System.Threading.Tasks;
using ConsumerService.DbContext;
using ConsumerService.Models;
using MongoDB.Driver;

namespace ConsumerService
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAccounts();
        Task<Account> GetAccount(string accountNumber);
        Task<Account> CreateAccount(Account account);
        Task UpdateAccount(string accountNumber, Account account);
    }
    public class AccountRepository : IAccountRepository
    {
        private readonly IMongoCollection<Account> _account;

        public AccountRepository(IAccountDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _account = database.GetCollection<Account>(settings.AccounsCollectionName);
        }

        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await _account.Find(a => true).ToListAsync();
        }
        public async Task<Account> GetAccount(string accountNumber)
        {
            return await _account.Find(a => a.Number == accountNumber).FirstOrDefaultAsync();
        }
        public async Task<Account> CreateAccount(Account account)
        {
            await _account.InsertOneAsync(account);
            return account;
        }
        public async Task UpdateAccount(string accountNumber, Account account)
        {
            await _account.ReplaceOneAsync(a => a.Number == accountNumber, account);
        }
    }
}