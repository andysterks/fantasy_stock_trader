using FantasyStockTrader.Core;
using FantasyStockTrader.Core.DatabaseContext;
using Microsoft.AspNetCore.Mvc;

namespace FantasyStockTrader.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly FantasyStockTraderContext _dbContext;
        private readonly IAuthContext _authContext;

        public WalletController(FantasyStockTraderContext dbContext,
            IAuthContext authContext)
        {
            _dbContext = dbContext;
            _authContext = authContext;
        }

        [HttpGet]
        public Wallet Get()
        {
            var wallet = _dbContext.Wallets.FirstOrDefault(x => x.AccountId == _authContext.Account.Id);
            return wallet;
        }
    }
}
