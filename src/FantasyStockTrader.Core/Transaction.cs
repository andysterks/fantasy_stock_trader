namespace FantasyStockTrader.Core
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
