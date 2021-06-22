using System;

namespace ConsumerService.Models.Request
{
    public class ConsumerAccountRequest
    {
        public string Number { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Action { get; set; }
        public DateTime UpdateLog { get; set; }
    }
}