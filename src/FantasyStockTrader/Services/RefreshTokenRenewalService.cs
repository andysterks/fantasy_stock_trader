using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace FantasyStockTrader.Web.Services
{
    public interface IRefreshTokenVerificationService
    {
        void Generate();
    }

    public class RefreshTokenVerificationService : IRefreshTokenVerificationService
    {
        private readonly IAuthCookieService _authCookieService;
        private readonly IAuthTokenCreationService _authTokenCreationService;
        private readonly IConfiguration _configuration;

        public RefreshTokenVerificationService(IAuthCookieService authCookieService, 
            IAuthTokenCreationService authTokenCreationService, 
            IConfiguration configuration)
        {
            _authCookieService = authCookieService;
            _authTokenCreationService = authTokenCreationService;
            _configuration = configuration;
        }

        public void Generate()
        {
            var refreshToken = _authCookieService.GetRefreshTokenFromCookie();
            if (refreshToken == null) throw new Exception("change this to custom exception");

            //var accessToken = _contextAccessor.HttpContext?.Request.Cookies[AccessTokenCookieId];
            //var refreshToken = _contextAccessor.HttpContext?.Request.Cookies[AccessTokenCookieId];

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var principal =
            //    tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
            //if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            //    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            //        StringComparison.InvariantCultureIgnoreCase)) throw new SecurityTokenException("Invalid token");

            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, "tbd"),
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
