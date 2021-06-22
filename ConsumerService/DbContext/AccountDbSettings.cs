namespace ConsumerService.DbContext
{
    public interface IAccountDbSettings
    {
        string AccounsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
    
    public class AccountDbSettings : IAccountDbSettings
    {
        public string AccounsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}