namespace FantasyStockTrader.Core
{
    public class Transaction
    {
        public long Id { get; set; }
        public Account Account { get; set; }
        public string Symbol { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
