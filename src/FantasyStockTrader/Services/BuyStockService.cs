using FantasyStockTrader.Core.DatabaseContext;
using FantasyStockTrader.Integration;

namespace FantasyStockTrader.Web.Services
{
    public interface IBuyStockService
    {
        Task BuyAsync(string symbol, int shares);
    }

    public class BuyStockService : IBuyStockService
    {
        private readonly IAuthContext _authContext;

        private readonly FantasyStockTraderContext _dbContext;
        private readonly IFinnhubApiService _finnhubApiService;
        private readonly IHoldingsUpdateService _holdingsUpdateService;
        private readonly IWalletUpdateService _walletUpdateService;
        private readonly ITransactionService _transactionService;

        public BuyStockService(IAuthContext authContext, 
            FantasyStockTraderContext dbContext, 
            IFinnhubApiService finnhubApiService, 
            IHoldingsUpdateService holdingsUpdateService, 
            IWalletUpdateService walletUpdateService, 
            ITransactionService transactionService)
        {
            _authContext = authContext;
            _dbContext = dbContext;
            _finnhubApiService = finnhubApiService;
            _holdingsUpdateService = holdingsUpdateService;
            _walletUpdateService = walletUpdateService;
            _transactionService = transactionService;
        }

        public async Task BuyAsync(string symbol, int shares)
        {
            var accountWallet = _dbContext.Wallets.FirstOrDefault(x => x.AccountId == _authContext.Account.Id);

            var quote = await _finnhubApiService.GetPriceAsync(symbol, _authContext.Account.Id);
            var costBasis = quote.CurrentPrice * shares;

            if (costBasis > (double)accountWallet.Amount)
            {
                throw new Exception("Transaction would exceed available funds");
            }

            _transactionService.AddBuy(symbol, shares, quote.CurrentPrice);
            _walletUpdateService.SubtractCurrency((decimal)costBasis);
            _holdingsUpdateService.UpdateWithBuy(_authContext.Account.Id, symbol, shares, costBasis);
        }
    }
}
