using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FantasyStockTrader.Integration
{
    public interface IFinnhubApiService
    {
        Task<FinnhubQuote> GetPrice(string symbol);
    }

    public class FinnhubApiService : IFinnhubApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubApiService(IHttpClientFactory httpClientFactory, 
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<FinnhubQuote> GetPrice(string symbol)
        {
            var client = _httpClientFactory.CreateClient();

            client.BaseAddress = new Uri("https://finnhub.io");

            var response =
                await client.GetAsync($"/api/v1/quote?symbol={symbol}&token={_configuration["Finnhub:Token"]}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error fetching stock price");
            }
            var body = await response.Content.ReadAsStringAsync();
            var quote = JsonConvert.DeserializeObject<FinnhubQuote>(await response.Content.ReadAsStringAsync());

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