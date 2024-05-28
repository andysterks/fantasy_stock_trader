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
        private readonly IAuthContext _authContext;

        public BuyController(IHttpClientFactory httpClientFactory, 
            FantasyStockTraderContext dbContext, 
            IAuthContext authContext)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
            _authContext = authContext;
        }

        [HttpGet("summary")]
        public BuySummary GetBuySummary([FromQuery] string symbol)
        {
            var accountWallet = _dbContext.Wallets.FirstOrDefault(x => x.AccountId == _authContext.Account.Id);

            var currentPrice = 57.85;

            var maxShareAmount = (int)Math.Floor((double)accountWallet.Amount / currentPrice); 

            return new BuySummary(currentPrice, maxShareAmount, accountWallet.Amount);
        }

        public record BuySummary(double CurrentPrice, int MaxShareAmount, decimal walletAmount);
    }
}
