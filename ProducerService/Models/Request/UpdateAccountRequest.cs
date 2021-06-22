using System;

namespace ProducerService.Models.Request
{
    public class UpdateAccountRequest
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}