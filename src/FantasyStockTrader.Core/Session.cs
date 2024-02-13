namespace FantasyStockTrader.Core
{
    public class Session
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
