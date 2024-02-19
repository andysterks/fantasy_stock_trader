using Microsoft.AspNetCore.Mvc;

namespace FantasyStockTrader.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost]
    public ActionResult Post([FromBody] LoginModel loginModel)
    {
        if (loginModel.EmailAddress == null || loginModel.Password == null)
            // TODO: Create custom exception for invalid incoming params
            throw new Exception();

        // check email against db
        if (loginModel.EmailAddress == "admin@email.com" && loginModel.Password == "1234")
        {
            // update cookie
        }

        // return data
        return Ok();
    }

    public class LoginModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}