using FantasyStockTrader.Core;
using FantasyStockTrader.Core.DatabaseContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FantasyStockTrader.Web.Controllers
{
    [Route("api/register-user")]
    [AllowAnonymous]
    [ApiController]
    public class RegisterUserController : ControllerBase
    {
        private readonly FantasyStockTraderContext _dbContext;

        public RegisterUserController(FantasyStockTraderContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] AccountInputModel accountInputModel)
        {
            var account = _dbContext.Accounts.FirstOrDefault(x => x.EmailAddress == accountInputModel.EmailAddress);
            if (account != null)
            {
                return Conflict("Account already exists");
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(accountInputModel.Password);
                    account = new Account
                    {
                        EmailAddress = accountInputModel.EmailAddress,
                        Password = hashedPassword,
                        FirstName = accountInputModel.FirstName,
                        LastName = accountInputModel.LastName
                    };

                    _dbContext.Accounts.Add(account);
                    _dbContext.SaveChanges();

                    var wallet = new Wallet
                    {
                        AccountId = account.Id,
                        Amount = 100000
                    };

                    _dbContext.Wallets.Add(wallet);
                    _dbContext.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return Ok();
        }
    }
}
