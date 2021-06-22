using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProducerService.Models.Configuration;
using ProducerService.Models.Response;

namespace ProducerService.Services
{
    public interface IConsumerService
    {
        Task<AccountResponse> GetAccount(string accountNumber);
    }

    public class ConsumerService : IConsumerService
    {
        public HttpClient Client { get; }
        private readonly string ConsumerServiceUrl;

        public ConsumerService(HttpClient client, IOptions<ServiceAddressConfiguration> serviceAddressConfiguration)
        {
            ConsumerServiceUrl = serviceAddressConfiguration.Value.ConsumerServiceUrl;
            Client = client;
        }

        public async Task<AccountResponse> GetAccount(string accountNumber)
        {
            var httpResponse = await Client.GetAsync($"{ConsumerServiceUrl}/api/Accounts/{accountNumber}");
            httpResponse.EnsureSuccessStatusCode();
            var content = await httpResponse.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AccountResponse>(content);
        }
    }
}