using System.Security.Claims;
using System.Security.Cryptography;
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

        public LoginService(IAuthTokenCreationService authTokenCreationService, 
            IAuthCookieService authCookieService)
        {
            _authTokenCreationService = authTokenCreationService;
            _authCookieService = authCookieService;
        }

        public void Login(string emailAddress, string password)
        {
            // TODO: replace with db check
            if (emailAddress == "admin@email.com" && password == "1234")
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
