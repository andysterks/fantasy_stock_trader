using FantasyStockTrader.Core;
using FantasyStockTrader.Core.DatabaseContext;
using FantasyStockTrader.Integration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FantasyStockTrader.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BuyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FantasyStockTraderContext _dbContext;
        private readonly IAuthContext _authContext;
        private readonly IFinnhubApiService _finnhubApiService;

        public BuyController(IHttpClientFactory httpClientFactory, 
            FantasyStockTraderContext dbContext, 
            IAuthContext authContext, 
            IFinnhubApiService finnhubApiService)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
            _authContext = authContext;
            _finnhubApiService = finnhubApiService;
        }

        [HttpGet("summary")]
        public async Task<BuySummary> GetBuySummary([FromQuery] string symbol)
        {
            var accountWallet = _dbContext.Wallets.FirstOrDefault(x => x.AccountId == _authContext.Account.Id);

            var stockQuote = await _finnhubApiService.GetPrice(symbol);
            var currentPrice = stockQuote.CurrentPrice;

            var maxShareAmount = (int)Math.Floor((double)accountWallet.Amount / currentPrice); 

            return new BuySummary(currentPrice, maxShareAmount, accountWallet.Amount);
        }

        [HttpPost("execute")]
        public async Task Execute([FromBody] BuyTransaction buyTransaction)
        {
            var accountWallet = _dbContext.Wallets.FirstOrDefault(x => x.AccountId == _authContext.Account.Id);

            var stockQuote = await _finnhubApiService.GetPrice(buyTransaction.Symbol);
            var currentPrice = stockQuote.CurrentPrice;

            // check if can purchase
            if (buyTransaction.Amount * currentPrice > (double)accountWallet.Amount)
            {
                throw new Exception("Transaction would exceed available funds");
            }

            _dbContext.Transactions.Add(new Transaction
            {
                AccountId = _authContext.Account.Id,
                Symbol = buyTransaction.Symbol,
                Type = "BUY",
                Amount = buyTransaction.Amount,
                Price = currentPrice,
                CreatedAt = DateTime.UtcNow
            });

            // subtract from wallet
            accountWallet.Amount -= (decimal)(buyTransaction.Amount * currentPrice);
            _dbContext.Wallets.Update(accountWallet);

            // add to holdings
            var holding = _dbContext.Holdings.FirstOrDefault(x => x.Symbol == buyTransaction.Symbol 
                                                                  && x.Account.Id == _authContext.Account.Id);

            if (holding == null)
            {
                _dbContext.Holdings.Add(new Holding
                {
                    AccountId = _authContext.Account.Id,
                    Symbol = buyTransaction.Symbol,
                    Shares = buyTransaction.Amount,
                    CostBasis = buyTransaction.Amount * currentPrice,
                    CreatedAt = DateTime.UtcNow
                });
            }
            else
            {
                holding.CostBasis += buyTransaction.Amount * currentPrice;
                holding.Shares += buyTransaction.Amount;
            }

            _dbContext.SaveChanges();
        }

        public record BuySummary(double CurrentPrice, int MaxShareAmount, decimal walletAmount);

        public record BuyTransaction
        {
            public string Symbol { get; set; }
            public int Amount { get; set; }
        };
    }
}
