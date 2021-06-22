using System.Threading.Tasks;
using MongoDB.Driver;
using ProducerService.DbContext;
using ProducerService.Models;

namespace ProducerService.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> CreateAccount(Account account);
        Task Deposit(Account account);
        Task Withdraw(Account account);
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

        public async Task<Account> CreateAccount(Account account)
        {
            await _account.InsertOneAsync(account);
            return account;
        }
        public async Task Deposit(Account account)
        {
            await _account.InsertOneAsync(account);
        }
        public async Task Withdraw(Account account)
        {
            await _account.InsertOneAsync(account);
        }
    }
}