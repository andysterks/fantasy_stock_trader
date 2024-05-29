using FantasyStockTrader.Core.DatabaseContext;

namespace FantasyStockTrader.Web.Services
{
    public interface IWalletUpdateService
    {
        void SubtractCurrency(decimal amount);
    }

    public class WalletUpdateService : IWalletUpdateService
    {
        private readonly IAuthContext _authContext;
        private readonly FantasyStockTraderContext _dbContext;

        public WalletUpdateService(IAuthContext authContext, 
            FantasyStockTraderContext dbContext)
        {
            _authContext = authContext;
            _dbContext = dbContext;
        }

        public void SubtractCurrency(decimal amount)
        {
            var accountWallet = _dbContext.Wallets.First(x => x.AccountId == _authContext.Account.Id);

            accountWallet.Amount -= amount;
            _dbContext.Wallets.Update(accountWallet);
        }
    }
}
