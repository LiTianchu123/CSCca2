namespace UserAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string Subscription { get; set; }
        
    }
}