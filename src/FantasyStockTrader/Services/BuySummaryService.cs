using FantasyStockTrader.Core.DatabaseContext;
using FantasyStockTrader.Integration;

namespace FantasyStockTrader.Web.Services
{
    public interface IBuySummaryService
    {
        Task<BuySummary> Get(string symbol);
    }

    public class BuySummaryService : IBuySummaryService
    {
        private readonly FantasyStockTraderContext _dbContext;
        private readonly IAuthContext _authContext;
        private readonly IFinnhubApiService _finnhubApiService;

        public BuySummaryService(FantasyStockTraderContext dbContext, 
            IAuthContext authContext, 
            IFinnhubApiService finnhubApiService)
        {
            _dbContext = dbContext;
            _authContext = authContext;
            _finnhubApiService = finnhubApiService;
        }

        public async Task<BuySummary> Get(string symbol)
        {
            var accountWallet = _dbContext.Wallets.FirstOrDefault(x => x.AccountId == _authContext.Account.Id);

            var stockQuote = await _finnhubApiService.GetPrice(symbol);
            var currentPrice = stockQuote.CurrentPrice;

            var maxShareAmount = (int)Math.Floor((double)accountWallet.Amount / currentPrice);

            return new BuySummary(currentPrice, maxShareAmount, accountWallet.Amount);
        }
    }

    public record BuySummary(double CurrentPrice, int MaxShareAmount, decimal walletAmount);
}
