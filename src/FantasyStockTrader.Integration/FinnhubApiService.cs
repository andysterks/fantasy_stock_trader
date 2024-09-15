using FantasyStockTrader.Core;
using FantasyStockTrader.Core.DatabaseContext;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FantasyStockTrader.Integration
{
    public interface IFinnhubApiService
    {
        Task<FinnhubQuote> GetPriceAsync(string symbol, Guid accountId);
    }

    public class FinnhubApiService : IFinnhubApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly FantasyStockTraderContext _dbContext;

        private const string BaseUrl = "https://finnhub.io";
        private const string ApiName = "Finnhub";

        public FinnhubApiService(IHttpClientFactory httpClientFactory, 
            IConfiguration configuration, 
            IMemoryCache memoryCache, 
            FantasyStockTraderContext dbContext)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _memoryCache = memoryCache;
            _dbContext = dbContext;
        }

        public async Task<FinnhubQuote> GetPriceAsync(string symbol, Guid accountId)
        {
            var endpoint = $"/api/v1/quote?symbol={symbol}&token={_configuration["Finnhub:Token"]}";
            var apiUrl = $"{BaseUrl}{endpoint}";

            if (_memoryCache.TryGetValue($"stock_price_{symbol}", out FinnhubQuote cachedQuote))
            {
                _dbContext.ExternalApiCalls.Add(new ExternalApiCall
                {
                    AccountId = accountId,
                    ApiName = ApiName,
                    Endpoint = endpoint,
                    IsCached = true
                });
                await _dbContext.SaveChangesAsync();
                return cachedQuote;
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
            await _dbContext.SaveChangesAsync();


            var response = await client.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error fetching stock price");
            }

            var quote = JsonConvert.DeserializeObject<FinnhubQuote>(await response.Content.ReadAsStringAsync());

            _memoryCache.Set($"stock_price_{symbol}", quote, DateTime.Now.AddMinutes(1));

            return quote;
        }
    }

    public record FinnhubQuote(
        [property: JsonProperty("c")] double CurrentPrice,
        [property: JsonProperty("d")] double Change,
        [property: JsonProperty("dp")] double PercentChange,
        [property: JsonProperty("h")] double HighPriceOfDay,
        [property: JsonProperty("l")] double LowPriceOfDay,
        [property: JsonProperty("o")] double OpenPriceOfDay,
        [property: JsonProperty("pc")] double PreviousClosePrice
    );
}