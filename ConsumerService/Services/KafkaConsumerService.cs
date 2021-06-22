using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using ConsumerService.Models.Configuration;
using ConsumerService.Models.Request;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ConsumerService.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly string topic;
        private readonly IAccountService _accountService;
        private readonly ConsumerConfig _consumerConfig;

        public KafkaConsumerService(IAccountService accountService, IOptions<KafkaConfiguration> kafkaConfiguration)
        {
            _accountService = accountService;
            topic = kafkaConfiguration.Value.Topic;

            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = kafkaConfiguration.Value.Host,
                GroupId = kafkaConfiguration.Value.GroupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(() => StartConsumer(stoppingToken));
            return Task.CompletedTask;
        }

        private Task StartConsumer(CancellationToken cancellationToken)
        {
            Console.WriteLine("Kafka consumer starting");

            while (!cancellationToken.IsCancellationRequested)
            {
                using (var consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig).Build())
                {
                    while (true)
                    {
                        consumer.Subscribe(topic);
                        var consumeResult = consumer.Consume();
                        if (consumeResult != null)
                        {
                            OnReceivedMessage(consumeResult);
                            Console.WriteLine($"Received message {consumeResult.Message.Value} from topic {topic}");
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }

        private async void OnReceivedMessage(ConsumeResult<Ignore, string> consume)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<ConsumerAccountRequest>(consume.Message.Value);
                if (result.Action == ActionEnum.OPEN_ACCOUNT.ToString())
                {
                    var account = new CreateAccountRequest
                    {
                        AccountNumber = result.Number,
                        AccountName = result.Name,
                        Amount = result.Amount,
                        UpdateDate = result.UpdateLog
                    };
                    await _accountService.CreateAccount(account);
                }
                if (result.Action == ActionEnum.DEPOSIT.ToString())
                {
                    var account = new UpdateAccountRequest
                    {
                        AccountNumber = result.Number,
                        Amount = result.Amount,
                        Action = result.Action,
                        UpdateDate = result.UpdateLog
                    };
                    await _accountService.UpdateAccount(account);
                }
                if (result.Action == ActionEnum.WITHDRAW.ToString())
                {
                    var account = new UpdateAccountRequest
                    {
                        AccountNumber = result.Number,
                        Amount = result.Amount,
                        Action = result.Action,
                        UpdateDate = result.UpdateLog
                    };
                    await _accountService.UpdateAccount(account);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}