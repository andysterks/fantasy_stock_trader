namespace FantasyStockTrader.Core
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
