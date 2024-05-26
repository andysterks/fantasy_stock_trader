namespace FantasyStockTrader.Core
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public Account Account { get; set; }
        public decimal Amount { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
