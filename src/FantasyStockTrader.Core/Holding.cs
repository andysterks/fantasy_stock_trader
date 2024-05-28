namespace FantasyStockTrader.Core
{
    public class Holding
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public string Symbol { get; set; }
        public int Shares { get; set; }
        public double CostBasis { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}