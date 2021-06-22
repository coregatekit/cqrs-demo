namespace ConsumerService.Models.Configuration
{
    public class KafkaConfiguration
    {
        public string Host { get; set; }
        public string GroupId { get; set; }
        public string Topic { get; set; }
    }
}