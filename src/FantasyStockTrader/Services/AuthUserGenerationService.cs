using FantasyStockTrader.Core.DatabaseContext;
using FantasyStockTrader.Core.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FantasyStockTrader.Web.Services
{
    public interface IAuthUserGenerationService
    {
        IAuthContext GetAuthContext();
    }

    public class AuthUserGenerationService : IAuthUserGenerationService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly FantasyStockTraderContext _dbContext;

        public AuthUserGenerationService(IHttpContextAccessor contextAccessor, 
                FantasyStockTraderContext dbContext)
        {
            _contextAccessor = contextAccessor;
            _dbContext = dbContext;
        }

        public IAuthContext GetAuthContext()
        {
            var cookie = _contextAccessor.HttpContext?.Request.Cookies["fst-access-id"];
            if (cookie == null)
            {
                throw new FSTAuthorizationException("Authorization missing");
            }

            var jwtTokenValue = new JwtSecurityTokenHandler().ReadJwtToken(cookie);
            var nameClaim = jwtTokenValue.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            if (nameClaim == null)
            {
                throw new FSTAuthorizationException("Authorization missing");
            }

            var account = _dbContext.Accounts.First(x => x.EmailAddress == nameClaim.Value);

            return new AuthContext
            {
                Account = account
            };
        } 
    }
}
