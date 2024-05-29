using FantasyStockTrader.Core.DatabaseContext;
using FantasyStockTrader.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FantasyStockTrader.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BuyController : ControllerBase
    {
        private readonly IBuySummaryService _buySummaryService;
        private readonly IBuyStockService _buyStockService;
        private readonly FantasyStockTraderContext _dbContext;

        public BuyController(
            IBuySummaryService buySummaryService,
            IBuyStockService buyStockService, 
            FantasyStockTraderContext dbContext)
        {
            _buySummaryService = buySummaryService;
            _buyStockService = buyStockService;
            _dbContext = dbContext;
        }

        [HttpGet("summary")]
        public async Task<BuySummary> GetBuySummary([FromQuery] string symbol)
        {
            return await _buySummaryService.Get(symbol);
        }

        [HttpPost("execute")]
        public async Task Execute([FromBody] BuyTransaction buyTransaction)
        {
            await _buyStockService.BuyAsync(buyTransaction.Symbol, buyTransaction.Amount);

            _dbContext.SaveChanges();
        }
        
        public record BuyTransaction
        {
            public string Symbol { get; set; }
            public int Amount { get; set; }
        };
    }
}
