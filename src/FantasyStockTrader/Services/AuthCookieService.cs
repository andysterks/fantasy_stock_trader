using System.IdentityModel.Tokens.Jwt;
using FantasyStockTrader.Core;

namespace FantasyStockTrader.Web.Services
{
    public interface IAuthCookieService
    {
        void SetAccessTokenCookie(JwtSecurityToken token);
        string? GetRefreshTokenFromCookie();
        void SetRefreshTokenCookie(Session session);
    }

    public class AuthCookieService : IAuthCookieService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        public const string AccessTokenCookieId = "fst-access-id";
        public const string RefreshCookieId = "fst-refresh-id";

        public AuthCookieService(IHttpContextAccessor contextAccessor,
            IConfiguration configuration)
        {
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        public void SetAccessTokenCookie(JwtSecurityToken token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Use this in production with HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = token.ValidTo
            };

            _contextAccessor.HttpContext?.Response.Cookies.Append(
                AccessTokenCookieId,
                new JwtSecurityTokenHandler().WriteToken(token),
                cookieOptions);
        }

        public string? GetRefreshTokenFromCookie()
        {
            return _contextAccessor.HttpContext?.Request.Cookies[RefreshCookieId];
        }

        public void SetRefreshTokenCookie(Session session)
        {
            int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out var refreshTokenValidityInDays);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Use this in production with HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddDays(refreshTokenValidityInDays)
            };

            _contextAccessor.HttpContext?.Response.Cookies.Append(
                RefreshCookieId,
                session.RefreshToken,
                cookieOptions);
        }
    }
}
