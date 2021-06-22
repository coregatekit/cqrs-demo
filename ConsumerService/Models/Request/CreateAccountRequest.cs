namespace ConsumerService.Models.Request
{
    public class CreateAccountRequest
    {
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
    }
}