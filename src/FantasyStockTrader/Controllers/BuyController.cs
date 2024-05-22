using FantasyStockTrader.Core.DatabaseContext;
using Microsoft.AspNetCore.Mvc;

namespace FantasyStockTrader.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FantasyStockTraderContext _dbContext;

        public BuyController(IHttpClientFactory httpClientFactory, 
            FantasyStockTraderContext dbContext)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
        }

        [HttpGet("summary")]
        public BuySummary GetBuySummary([FromQuery] string symbol)
        {
            var walletBalance = 100000.00;

            var currentPrice = 57.85;

            var maxShareAmount = (int)Math.Floor(walletBalance / currentPrice); 

            return new BuySummary(currentPrice, maxShareAmount);
        }

        public record BuySummary(double CurrentPrice, int MaxShareAmount);
    }
}
