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
            
            //var response = await client.GetAsync($"/api/v3/search?query={query}&limit=10&apikey={_configuration["FinancialModelingPrep:Token"]}");

            //if (!response.IsSuccessStatusCode)
            //{
            //    throw new Exception("Error fetching companies from Financial Modeling Prep API");
            //}

            var responseBody =
                "[\r\n  {\r\n    \"symbol\": \"FB\",\r\n    \"name\": \"Meta Platforms, Inc.\",\r\n    \"currency\": \"USD\",\r\n    \"stockExchange\": \"NASDAQ Global Select\",\r\n    \"exchangeShortName\": \"NASDAQ\"\r\n  },\r\n  {\r\n    \"symbol\": \"FBZ\",\r\n    \"name\": \"First Trust Brazil AlphaDEX Fund\",\r\n    \"currency\": \"USD\",\r\n    \"stockExchange\": \"NASDAQ Global Market\",\r\n    \"exchangeShortName\": \"NASDAQ\"\r\n  },\r\n  {\r\n    \"symbol\": \"FBY\",\r\n    \"name\": \"YieldMax META Option Income Strategy ETF\",\r\n    \"currency\": \"USD\",\r\n    \"stockExchange\": \"New York Stock Exchange Arca\",\r\n    \"exchangeShortName\": \"AMEX\"\r\n  },\r\n  {\r\n    \"symbol\": \"FBT\",\r\n    \"name\": \"First Trust NYSE Arca Biotechnology Index Fund\",\r\n    \"currency\": \"USD\",\r\n    \"stockExchange\": \"New York Stock Exchange Arca\",\r\n    \"exchangeShortName\": \"AMEX\"\r\n  },\r\n  {\r\n    \"symbol\": \"FBP\",\r\n    \"name\": \"First BanCorp.\",\r\n    \"currency\": \"USD\",\r\n    \"stockExchange\": \"New York Stock Exchange\",\r\n    \"exchangeShortName\": \"NYSE\"\r\n  },\r\n  {\r\n    \"symbol\": \"FBL\",\r\n    \"name\": \"GraniteShares 1.5x Long META Daily ETF\",\r\n    \"currency\": \"USD\",\r\n    \"stockExchange\": \"NASDAQ Global Market\",\r\n    \"exchangeShortName\": \"NASDAQ\"\r\n  },\r\n  {\r\n    \"symbol\": \"FBK\",\r\n    \"name\": \"FB Financial Corporation\",\r\n    \"currency\": \"USD\",\r\n    \"stockExchange\": \"New York Stock Exchange\",\r\n    \"exchangeShortName\": \"NYSE\"\r\n  },\r\n  {\r\n    \"symbol\": \"FBC\",\r\n    \"name\": \"Flagstar Bancorp, Inc.\",\r\n    \"currency\": \"USD\",\r\n    \"stockExchange\": \"New York Stock Exchange\",\r\n    \"exchangeShortName\": \"NYSE\"\r\n  },\r\n  {\r\n    \"symbol\": \"FBYD\",\r\n    \"name\": \"Falcon's Beyond Global, Inc. Class A Common Stock\",\r\n    \"currency\": \"USD\",\r\n    \"stockExchange\": \"NASDAQ\",\r\n    \"exchangeShortName\": \"NASDAQ\"\r\n  },\r\n  {\r\n    \"symbol\": \"FBVI\",\r\n    \"name\": \"FCN Banc Corp.\",\r\n    \"currency\": \"USD\",\r\n    \"stockExchange\": \"Other OTC\",\r\n    \"exchangeShortName\": \"PNK\"\r\n  }\r\n]\r\n";
            //var responseBody = await response.Content.ReadAsStringAsync();
            var companyModels = JsonConvert.DeserializeObject<List<CompanyModel>>(responseBody);

            return companyModels;
        }

        public record CompanyModel(string symbol, string name, string currency);
    }
}
