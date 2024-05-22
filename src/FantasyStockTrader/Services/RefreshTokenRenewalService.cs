using FantasyStockTrader.Core.DatabaseContext;
using FantasyStockTrader.Core.Exceptions;
using System.Security.Cryptography;
using FantasyStockTrader.Core;
using Microsoft.EntityFrameworkCore;

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
        private readonly FantasyStockTraderContext _dbContext;
        private readonly IConfiguration _configuration;

        public RefreshTokenRenewalService(IAuthCookieService authCookieService, 
            IAuthTokenCreationService authTokenCreationService, 
            FantasyStockTraderContext dbContext, IConfiguration configuration)
        {
            _authCookieService = authCookieService;
            _authTokenCreationService = authTokenCreationService;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public void Renew()
        {
            var refreshToken = _authCookieService.GetRefreshTokenFromCookie();
            VerifyRefreshToken(refreshToken);

            var matchingSession = _dbContext.Sessions
                .Include(s => s.Account)
                .FirstOrDefault(x => x.RefreshToken == refreshToken);
            VerifySessionExists(matchingSession);
            VerifyRefreshTokenIsCurrent(matchingSession!);
            
            var newAccessToken = _authTokenCreationService.CreateToken(matchingSession!.Account.EmailAddress);
            var newRefreshToken = GenerateRefreshToken();

            // store refresh token in db
            int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out var refreshTokenValidityInDays);
            var session = new Session
            {
                Account = matchingSession.Account,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenValidityInDays)
            };
            _dbContext.Sessions.Add(session);
            _dbContext.SaveChanges();

            _authCookieService.SetAccessTokenCookie(newAccessToken);
            _authCookieService.SetRefreshTokenCookie(session);
        }

        private void VerifyRefreshToken(string? refreshToken)
        {
            if (refreshToken == null)
            {
                throw new FSTAuthorizationException("Refresh token is missing.");
            }
        }

        private void VerifySessionExists(Session? session)
        {
            if (session is null)
            {
                throw new FSTAuthorizationException("No matching refresh token");
            }
        }

        private void VerifyRefreshTokenIsCurrent(Session session)
        {
            if (session.IsExpired())
            {
                throw new FSTAuthorizationException("Refresh token is expired");
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
