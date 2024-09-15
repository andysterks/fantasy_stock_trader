using FantasyStockTrader.Core;
using FantasyStockTrader.Core.DatabaseContext;
using FantasyStockTrader.Integration;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet("summary")]
    public AccountSummaryModel GetSummary([FromQuery] int page = 1, [FromQuery] int pageSize = 8)
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
            .OrderByDescending(x => x.Value)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var totalHoldings = _dbContext.Holdings.Count(x => x.AccountId == _authContext.Account.Id);
        var totalPages = (int)Math.Ceiling((double)totalHoldings / pageSize);

        var accountValue = _dbContext.Holdings
            .Where(x => x.AccountId == _authContext.Account.Id)
            .Sum(x => (double)(_finnhubApiService.GetPrice(x.Symbol).Result.CurrentPrice * x.Shares));
        var accountCostBasis = _dbContext.Holdings
            .Where(x => x.AccountId == _authContext.Account.Id)
            .Sum(x => x.CostBasis);

        return new AccountSummaryModel(
            (double)wallet.Amount,
            holdings,
            accountValue,
            accountCostBasis,
            accountValue - accountCostBasis,
            page,
            totalPages
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
    double AccountPerformance,
    int CurrentPage,
    int TotalPages
);

public record HoldingsModel
{
    public string Symbol { get; set; }
    public int SharesAmount { get; set; }
    public double CostBasis { get; set; }
    public double Value { get; set; }
    public double Performance => Value - CostBasis;
}