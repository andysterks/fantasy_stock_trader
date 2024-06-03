namespace FantasyStockTrader.Core
{
    public class Wallet
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public Account Account { get; set; }
        public decimal Amount { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
