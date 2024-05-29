using FantasyStockTrader.Core.DatabaseContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FantasyStockTrader.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CompanyController(IHttpClientFactory httpClientFactory, 
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<List<CompanyModel>> Get([FromQuery] string query)
        {
            var client = _httpClientFactory.CreateClient();
            
            client.BaseAddress = new Uri("https://financialmodelingprep.com");

            var response = await client.GetAsync($"/api/v3/search?query={query}&exchange=NASDAQ&apikey={_configuration["FinancialModelingPrep:Token"]}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error fetching companies from Financial Modeling Prep API");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var companyModels = JsonConvert.DeserializeObject<List<CompanyModel>>(responseBody);

            return companyModels;
        }

        public record CompanyModel(string symbol, string name, string currency, string stockExchange);
    }
}
