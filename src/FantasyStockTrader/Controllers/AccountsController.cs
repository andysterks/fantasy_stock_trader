using FantasyStockTrader.Core.DatabaseContext;
using Microsoft.AspNetCore.Mvc;

namespace FantasyStockTrader.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly FantasyStockTraderContext _fantasyStockTraderContext;

        public AccountsController(FantasyStockTraderContext fantasyStockTraderContext)
        {
            _fantasyStockTraderContext = fantasyStockTraderContext;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AccountInputModel accountInputModel)
        {
            var accounts = _fantasyStockTraderContext.Accounts.ToList();

            return Ok();
        }
    }

    public class AccountInputModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
