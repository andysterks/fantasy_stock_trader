namespace FantasyStockTrader.Core
{
    public class Account
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        
        public IList<Holding> Holdings { get; set; }
    }
}
