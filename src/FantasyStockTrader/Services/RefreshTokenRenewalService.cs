using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FantasyStockTrader.Core.DatabaseContext;
using FantasyStockTrader.Core.Exceptions;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace FantasyStockTrader.Web.Services
{
    public interface IRefreshTokenRenewalService
    {
        void Renew();
    }

    public class RefreshTokenRenewalService : IRefreshTokenRenewalService
    {
        private readonly IAuthCookieService _authCookieService;
        private readonly IAuthTokenCreationService _authTokenCreationService;
        private readonly IConfiguration _configuration;
        private readonly FantasyStockTraderContext _dbContext;

        public RefreshTokenRenewalService(IAuthCookieService authCookieService, 
            IAuthTokenCreationService authTokenCreationService, 
            IConfiguration configuration, 
            FantasyStockTraderContext dbContext)
        {
            _authCookieService = authCookieService;
            _authTokenCreationService = authTokenCreationService;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public void Renew()
        {
            var refreshToken = _authCookieService.GetRefreshTokenFromCookie();
            if (refreshToken == null)
            {
                throw new FTSAuthorizationException("Refresh token is missing.");
            }

            var matchingSession = _dbContext.Sessions.FirstOrDefault(x => x.Id.ToString() == refreshToken);
            if (matchingSession is null)
            {
                throw new FTSAuthorizationException("No matching refresh token");
            }

            if (matchingSession.ExpiresAt > DateTime.UtcNow)
            {
                throw new FTSAuthorizationException("Refresh token is expired");
            }

            //var tokenValidationParameters = new TokenValidationParameters
            //{
            //    ValidateAudience = false,
            //    ValidateIssuer = false,
            //    ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
            //    ValidateLifetime = false
            //};

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var principal =
            //    tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
            //if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            //    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            //        StringComparison.InvariantCultureIgnoreCase)) throw new SecurityTokenException("Invalid token");

            var authClaims = new List<Claim>
            {
                // TODO: change to email address once account is linked
                new(ClaimTypes.Name, matchingSession.AccountId.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var newAccessToken = _authTokenCreationService.CreateToken(authClaims);
            var newRefreshToken = GenerateRefreshToken();

            _authCookieService.SetAccessTokenCookie(newAccessToken);
            _authCookieService.SetRefreshTokenCookie(newRefreshToken);
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
