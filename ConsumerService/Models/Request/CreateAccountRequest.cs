using System;

namespace ConsumerService.Models.Request
{
    public class CreateAccountRequest
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}