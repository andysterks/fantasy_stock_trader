namespace FantasyStockTrader.Core
{
    public class ExternalApiCall
    {
        public Int64 Id { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public string ApiName { get; set; }
        public string Endpoint { get; set; }
        public bool IsCached { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
