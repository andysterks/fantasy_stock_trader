using FantasyStockTrader.Core;
using FantasyStockTrader.Core.DatabaseContext;
using FantasyStockTrader.Integration;
using Microsoft.AspNetCore.Mvc;

namespace FantasyStockTrader.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly FantasyStockTraderContext _dbContext;
    private readonly IAuthContext _authContext;
    private readonly IFinnhubApiService _finnhubApiService;

    public AccountsController(FantasyStockTraderContext dbContext,
        IAuthContext authContext,
        IFinnhubApiService finnhubApiService)
    {
        _dbContext = dbContext;
        _authContext = authContext;
        _finnhubApiService = finnhubApiService;
    }

    [HttpPost]
    public IActionResult Post([FromBody] AccountInputModel accountInputModel)
    {
        var accounts = _dbContext.Accounts.ToList();

        return Ok();
    }

    [HttpGet("summary")]
    public AccountSummaryModel GetSummary()
    {
        var wallet = _dbContext.Wallets.First(x => x.AccountId == _authContext.Account.Id);

        var holdings = _dbContext.Holdings
            .Where(x => x.AccountId == _authContext.Account.Id)
            .Select(x => new HoldingsModel
            {
                Symbol = x.Symbol,
                SharesAmount = x.Shares,
                CostBasis = x.CostBasis,
                Value = (double)(_finnhubApiService.GetPrice(x.Symbol).Result.CurrentPrice * x.Shares)
            })
            //.Select(x => new HoldingsModel(x.Symbol, x.Shares, x.CostBasis, x.Shares * stockQuote.CurrentPrice, x.Shares * stockQuote.CurrentPrice - x.CostBasis))
            .ToList();


        var accountValue = holdings.Sum(x => x.Value);
        var accountCostBasis = holdings.Sum(x => x.CostBasis);

        return new AccountSummaryModel(
            (double)wallet.Amount,
            holdings,
            accountValue,
            accountCostBasis,
            accountValue - accountCostBasis
            );
    }
}

public class AccountInputModel
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public record AccountSummaryModel(
    double WalletAmount,
    List<HoldingsModel> Holdings,
    double AccountValue,
    double AccountCostBasis,
    double AccountPerformance
    );

public record HoldingsModel
{
    public string Symbol { get; set; }
    public int SharesAmount { get; set; }
    public double CostBasis { get; set; }
    public double Value { get; set; }
    public double Performance => Value - CostBasis;
}