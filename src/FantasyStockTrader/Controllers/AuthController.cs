using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FantasyStockTrader.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _contextAccessor;
    public const string AccessTokenCookieId = "fst-access-id";
    public const string RefreshCookieId = "fst-refresh-id";

    public AuthController(IConfiguration configuration,
        IHttpContextAccessor contextAccessor)
    {
        _configuration = configuration;
        _contextAccessor = contextAccessor;
    }

    [HttpPost]
    public ActionResult Post([FromBody] LoginModel loginModel)
    {
        if (loginModel.EmailAddress == null || loginModel.Password == null)
            // TODO: Create custom exception for invalid incoming params
            throw new Exception();

        // check email against db
        if (loginModel.EmailAddress == "admin@email.com" && loginModel.Password == "1234")
        {
            var authClaims = new List<Claim>
            {
                new(ClaimTypes.Name, loginModel.EmailAddress),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var accessToken = CreateToken(authClaims);
            var refreshToken = GenerateRefreshToken();

            SetAccessTokenCookie(accessToken);
            SetRefreshTokenCookie(refreshToken);

            int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out var refreshTokenValidityInDays);
            return Ok();
        }
        
        return Ok();
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
    {
        if (tokenModel == null) return BadRequest("Invalid client requests");

        var accessToken = _contextAccessor.HttpContext?.Request.Cookies[AccessTokenCookieId];
        var refreshToken = _contextAccessor.HttpContext?.Request.Cookies[AccessTokenCookieId];

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal =
            tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase)) throw new SecurityTokenException("Invalid token");

        var newAccessToken = CreateToken(principal.Claims.ToList());
        var newRefreshToken = GenerateRefreshToken();

        SetAccessTokenCookie(newAccessToken);
        SetRefreshTokenCookie(newRefreshToken);

        return Ok();
    }

    private void SetAccessTokenCookie(JwtSecurityToken token)
    {
        var cookieOptions = new AuthCookieOptions(_configuration);

        _contextAccessor.HttpContext?.Response.Cookies.Append(
            AccessTokenCookieId,
            new JwtSecurityTokenHandler().WriteToken(token),
            cookieOptions);
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new AuthCookieOptions(_configuration);

        _contextAccessor.HttpContext?.Response.Cookies.Append(
            RefreshCookieId,
            refreshToken,
            cookieOptions);
    }

    public class AuthCookieOptions : CookieOptions
    {
        public AuthCookieOptions(IConfiguration configuration)
        {
            HttpOnly = true;
            int.TryParse(configuration["JWT:RefreshTokenValidityInDays"], out var refreshTokenValidityInDays);
            Expires = DateTime.Now.AddDays(refreshTokenValidityInDays);
        }
    }

    public class TokenModel
    {
        public string? RefreshToken { get; set; }
    }

    private JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out var tokenValidityInMinutes);
        var token = new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        return token;
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public class LoginModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}