using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FantasyStockTrader.Web.Services
{
    public interface IAuthTokenCreationService
    {
        JwtSecurityToken CreateToken(string emailAddress);
    }

    public class AuthTokenCreationService : IAuthTokenCreationService
    {
        private readonly IConfiguration _configuration;

        public AuthTokenCreationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtSecurityToken CreateToken(string emailAddress)
        {
            var authClaims = new List<Claim>
            {
                // TODO: change to email address once account is linked
                new(ClaimTypes.Name, emailAddress),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out var tokenValidityInMinutes);
            var token = new JwtSecurityToken(
                _configuration["JWT:ValidIssuer"],
                _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
