using FantasyStockTrader.Core;
using FantasyStockTrader.Core.DatabaseContext;

namespace FantasyStockTrader.Web.Services
{
    public interface IHoldingsUpdateService
    {
        void UpdateWithBuy(long accountId, string symbol, int shares, double costBasis);
    }

    public class HoldingsUpdateService : IHoldingsUpdateService
    {
        private readonly FantasyStockTraderContext _dbContext;

        public HoldingsUpdateService(FantasyStockTraderContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void UpdateWithBuy(long accountId, string symbol, int shares, double costBasis) 
        {
            var holding = _dbContext.Holdings.FirstOrDefault(x => x.Symbol == symbol
                                                                  && x.Account.Id == accountId);

            if (holding == null)
            {
                _dbContext.Holdings.Add(new Holding
                {
                    //Account = account,
                    Symbol = symbol,
                    Shares = shares,
                    CostBasis = costBasis,
                    CreatedAt = DateTime.UtcNow
                });
                return;
            }

            holding.CostBasis += costBasis;
            holding.Shares += shares;
        }
    }
}
