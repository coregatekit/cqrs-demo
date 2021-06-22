namespace ConsumerService.Models.Request
{
    public class UpdateAccountRequest
    {
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Action { get; set; }
    }
}