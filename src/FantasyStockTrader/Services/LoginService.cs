using FantasyStockTrader.Core;
using FantasyStockTrader.Core.DatabaseContext;
using FantasyStockTrader.Core.Exceptions;
using System.Security.Cryptography;

namespace FantasyStockTrader.Web.Services;

public interface ILoginService
{
    void Login(string emailAddress, string password);
}

public class LoginService : ILoginService
{
    private readonly IAuthTokenCreationService _authTokenCreationService;
    private readonly IAuthCookieService _authCookieService;
    private readonly FantasyStockTraderContext _dbContext;
    private readonly IConfiguration _configuration;

    public LoginService(IAuthTokenCreationService authTokenCreationService,
        IAuthCookieService authCookieService,
        FantasyStockTraderContext dbContext,
        IConfiguration configuration)
    {
        _authTokenCreationService = authTokenCreationService;
        _authCookieService = authCookieService;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public void Login(string emailAddress, string password)
    {
        // TODO: replace with db check
        var matchingUser = _dbContext.Accounts.FirstOrDefault(x => x.EmailAddress == emailAddress);
        if (matchingUser is null || matchingUser.Password != password)
            throw new FSTAuthorizationException("Email/password combination is not correct.");

        var accessToken = _authTokenCreationService.CreateToken(emailAddress);
        var refreshToken = GenerateRefreshToken();

        int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out var refreshTokenValidityInDays);
        var session = new Session
        {
            Account = matchingUser,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenValidityInDays)
        };

        _dbContext.Sessions.Add(session);
        _dbContext.SaveChanges();

        _authCookieService.SetAccessTokenCookie(accessToken);
        _authCookieService.SetRefreshTokenCookie(session);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}