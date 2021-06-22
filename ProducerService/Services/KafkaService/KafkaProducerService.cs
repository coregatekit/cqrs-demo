using System;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using ProducerService.Models.Configuration;

namespace ProducerService.Services.Producer
{
    public class KafkaProducerService
    {
        private readonly ProducerConfig config;
        private readonly string topic;

        public KafkaProducerService(IOptions<KafkaConfiguration> kafkaConfig)
        {
            config = new ProducerConfig
            {
                BootstrapServers = kafkaConfig.Value.Host
            };
            topic = kafkaConfig.Value.Topic;
        }

        public void produceToKakfa(string message)
        {
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
                    Console.WriteLine($"Produce message {message} to topic {topic}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Something went wron: {e}");
                }
            }
        }
    }
}