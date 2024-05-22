namespace FantasyStockTrader.Core
{
    public class Holding
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}