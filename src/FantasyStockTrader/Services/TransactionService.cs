using FantasyStockTrader.Core;
using FantasyStockTrader.Core.DatabaseContext;

namespace FantasyStockTrader.Web.Services
{
    public interface ITransactionService
    {
        void AddBuy(string symbol, int shares, double currentPrice);
    }

    public class TransactionService : ITransactionService
    {
        private readonly IAuthContext _authContext;
        private readonly FantasyStockTraderContext _dbContext;

        public TransactionService(IAuthContext authContext, 
            FantasyStockTraderContext dbContext)
        {
            _authContext = authContext;
            _dbContext = dbContext;
        }

        public void AddBuy(string symbol, int shares, double currentPrice)
        {
            _dbContext.Transactions.Add(new Transaction
            {
                Account = _authContext.Account,
                Symbol = symbol,
                Type = "BUY",
                Amount = shares,
                Price = currentPrice,
                CreatedAt = DateTime.UtcNow
            });
        }
    }
}
