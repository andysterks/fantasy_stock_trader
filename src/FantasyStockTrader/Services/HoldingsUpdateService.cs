using FantasyStockTrader.Core;
using FantasyStockTrader.Core.DatabaseContext;

namespace FantasyStockTrader.Web.Services
{
    public interface IHoldingsUpdateService
    {
        void UpdateWithBuy(Guid accountId, string symbol, int shares, double costBasis);
    }

    public class HoldingsUpdateService : IHoldingsUpdateService
    {
        private readonly FantasyStockTraderContext _dbContext;

        public HoldingsUpdateService(FantasyStockTraderContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void UpdateWithBuy(Guid accountId, string symbol, int shares, double costBasis) 
        {
            var holding = GetOrCreateHolding(accountId, symbol);
            UpdateHolding(holding, shares, costBasis);
            SaveChanges();
        }

        private Holding GetOrCreateHolding(Guid accountId, string symbol)
        {
            return _dbContext.Holdings.FirstOrDefault(x => x.Symbol == symbol && x.Account.Id == accountId)
                ?? CreateNewHolding(accountId, symbol);
        }

        private Holding CreateNewHolding(Guid accountId, string symbol)
        {
            var newHolding = new Holding
            {
                AccountId = accountId,
                Symbol = symbol,
                Shares = 0,
                CostBasis = 0,
                CreatedAt = DateTime.UtcNow
            };
            _dbContext.Holdings.Add(newHolding);
            return newHolding;
        }

        private void UpdateHolding(Holding holding, int shares, double costBasis)
        {
            holding.CostBasis += costBasis;
            holding.Shares += shares;
        }

        private void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
