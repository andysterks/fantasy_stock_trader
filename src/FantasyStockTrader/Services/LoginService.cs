using System.Security.Claims;
using System.Security.Cryptography;
using FantasyStockTrader.Core.DatabaseContext;
using FantasyStockTrader.Core.Exceptions;
using Microsoft.IdentityModel.JsonWebTokens;

namespace FantasyStockTrader.Web.Services
{
    public interface ILoginService
    {
        void Login(string emailAddress, string password);
    }

    public class LoginService : ILoginService
    {
        private readonly IAuthTokenCreationService _authTokenCreationService;
        private readonly IAuthCookieService _authCookieService;
        private readonly FantasyStockTraderContext _dbContext;

        public LoginService(IAuthTokenCreationService authTokenCreationService, 
            IAuthCookieService authCookieService, 
            FantasyStockTraderContext dbContext)
        {
            _authTokenCreationService = authTokenCreationService;
            _authCookieService = authCookieService;
            _dbContext = dbContext;
        }

        public void Login(string emailAddress, string password)
        {
            // TODO: replace with db check
            var matchingUser = _dbContext.Accounts.FirstOrDefault(x => x.EmailAddress == emailAddress);
            if (matchingUser is null || matchingUser.Password != password)
            {
                throw new FTSAuthorizationException("Email/password combination is not correct.");
            }

            if (matchingUser != null && matchingUser.Password == password)
            {
                var authClaims = new List<Claim>
                {
                    new(ClaimTypes.Name, emailAddress),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var accessToken = _authTokenCreationService.CreateToken(authClaims);
                var refreshToken = GenerateRefreshToken();

                _authCookieService.SetAccessTokenCookie(accessToken);
                _authCookieService.SetRefreshTokenCookie(refreshToken);
                
            }
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
