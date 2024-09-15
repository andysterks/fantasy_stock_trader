using FantasyStockTrader.Web.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FantasyStockTrader.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FantasyStockTrader.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILoginService _loginService;
    private readonly IRefreshTokenRenewalService _refreshTokenRenewalService;

    public AuthController(ILoginService loginService,
        IRefreshTokenRenewalService refreshTokenRenewalService)
    {
        _loginService = loginService;
        _refreshTokenRenewalService = refreshTokenRenewalService;
    }

    [HttpPost]
    public void Post([FromBody] LoginModel loginModel)
    {
        if (loginModel.EmailAddress == null || loginModel.Password == null)
            // TODO: Create custom exception for invalid incoming params
            throw new Exception();

        _loginService.Login(loginModel.EmailAddress, loginModel.Password);
    }

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var newAccessToken = _refreshTokenRenewalService.Renew();
        return Ok(new { accessToken = newAccessToken });
    }

    public class LoginModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }

    public class AccountModel
    {
        public Guid Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
    }
}