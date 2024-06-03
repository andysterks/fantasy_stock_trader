namespace FantasyStockTrader.Core
{
    public class Session
    {
        public long Id { get; set; }
        public Account Account { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public bool IsExpired()
        {
            return DateTime.UtcNow > ExpiresAt;
        }
    }
}
