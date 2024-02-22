using FantasyStockTrader.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FantasyStockTrader.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILoginService _loginService;
    private readonly IRefreshTokenVerificationService _refreshTokenVerificationService;

    public AuthController(ILoginService loginService, 
        IRefreshTokenVerificationService refreshTokenVerificationService)
    {
        _loginService = loginService;
        _refreshTokenVerificationService = refreshTokenVerificationService;
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
        _refreshTokenVerificationService.Generate();

        return Ok();
    }

    public class LoginModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}