using FantasyStockTrader.Core.DatabaseContext;
using FantasyStockTrader.Integration;

namespace FantasyStockTrader.Web.Services
{
    public interface IHoldingsSummaryService
    {
        double GetAccountValue(Guid accountId);
        double GetAccountCostBasis(Guid accountId);
    }

    public class HoldingsSummaryService : IHoldingsSummaryService
    {
        private readonly FantasyStockTraderContext _dbContext;
        private readonly IFinnhubApiService _finnhubApiService;

        public HoldingsSummaryService(
            FantasyStockTraderContext dbContext, 
            IFinnhubApiService finnhubApiService)
        {
            _dbContext = dbContext;
            _finnhubApiService = finnhubApiService;
        }

        public double GetAccountValue(Guid accountId)
        {
            var holdings = _dbContext.Holdings
                .Where(x => x.AccountId == accountId)
                .ToList();

            return holdings.Sum(x => (double)(_finnhubApiService.GetPriceAsync(x.Symbol).Result.CurrentPrice * x.Shares));
            
        }

        public double GetAccountCostBasis(Guid accountId)
        {
            return _dbContext.Holdings
                .Where(x => x.AccountId == accountId)
                .Sum(x => x.CostBasis);
        }
    }
}
