using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using FantasyStockTrader.Core.DatabaseContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FantasyStockTrader.Web
{
    public class CookieAuthenticationOptions : AuthenticationSchemeOptions { }

    public class CookieAuthenticationHandler : AuthenticationHandler<CookieAuthenticationOptions>
    {
        private readonly FantasyStockTraderContext _dbContext;
        private readonly IConfiguration _configuration;

        public CookieAuthenticationHandler(IOptionsMonitor<CookieAuthenticationOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock, 
            FantasyStockTraderContext dbContext, 
            IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Cookies.ContainsKey("fst-access-id"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
            }

            var cookie = Request.Cookies.FirstOrDefault(x => x.Key == "fst-access-id");
            if (string.IsNullOrEmpty(cookie.Value))
            {
                return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
            }
            
            return Task.FromResult(ValidateJwtToken(cookie.Value));
        }

        private AuthenticateResult ValidateJwtToken(string jwtValue)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParams = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                tokenHandler.ValidateToken(jwtValue, tokenValidationParams, out var validatedJwtToken);
            }
            catch (SecurityTokenException ex)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var jwtToken = tokenHandler.ReadJwtToken(jwtValue);

            var nameClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
            var account = _dbContext.Accounts.First(x => x.EmailAddress == nameClaim.Value);

            if (account == null || jwtToken.ValidTo <= DateTime.UtcNow)
            {
                return AuthenticateResult.Fail("Unauthorized");
            }

            var identity = new ClaimsIdentity(jwtToken.Claims, Scheme.Name);
            var principal = new System.Security.Principal.GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
