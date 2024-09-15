using FantasyStockTrader.Integration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FantasyStockTrader.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IAuthContext _authContext;
        private readonly IFinancialModelingPrepApiService _financialModelingPrepService;

        public CompanyController(IAuthContext authContext,
            IFinancialModelingPrepApiService financialModelingPrepService)
        {
            _financialModelingPrepService = financialModelingPrepService;
            _authContext = authContext;
        }

        [HttpGet]
        public async Task<List<CompanyModel>> Get([FromQuery] string query)
        {
            return await _financialModelingPrepService.SearchAsync(_authContext.Account.Id, query);
        }
    }
}
