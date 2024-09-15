using FantasyStockTrader.Core;
using FantasyStockTrader.Core.DatabaseContext;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyStockTrader.Integration
{
    public interface IFinancialModelingPrepApiService
    {
        Task<List<CompanyModel>> SearchAsync(Guid accountId, string query);
    }

    public class FinancialModelingPrepApiService : IFinancialModelingPrepApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly FantasyStockTraderContext _dbContext;

        private const string BaseUrl = "https://financialmodelingprep.com";
        private const string ApiName = "FinancialModelingPrep";

        public FinancialModelingPrepApiService(IHttpClientFactory httpClientFactory, 
            IConfiguration configuration, 
            IMemoryCache memoryCache, 
            FantasyStockTraderContext dbContext)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _memoryCache = memoryCache;
            _dbContext = dbContext;
        }

        public async Task<List<CompanyModel>> SearchAsync(Guid accountId, string query)
        {
            var endpoint =
                $"/api/v3/search?query={query}&exchange=NASDAQ&apikey={_configuration["FinancialModelingPrep:Token"]}";

            if (_memoryCache.TryGetValue($"search_{query}", out List<CompanyModel> companies))
            {
                _dbContext.ExternalApiCalls.Add(new ExternalApiCall
                {
                    AccountId = accountId,
                    ApiName = ApiName,
                    Endpoint = endpoint,
                    IsCached = true
                });
                return companies;
            }

            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri(BaseUrl);

            _dbContext.ExternalApiCalls.Add(new ExternalApiCall
            {
                AccountId = accountId,
                ApiName = ApiName,
                Endpoint = endpoint,
                IsCached = false
            });
            var response = await client.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error fetching companies from Financial Modeling Prep API");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            var companyModels = JsonConvert.DeserializeObject<List<CompanyModel>>(responseBody);

            return companyModels;
        }
    }

    public record CompanyModel(string symbol, string name, string currency, string stockExchange);
}
