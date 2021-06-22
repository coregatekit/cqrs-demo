using System;

namespace ProducerService.Models.Response
{
    public class AccountResponse
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}