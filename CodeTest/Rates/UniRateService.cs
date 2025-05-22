using CodeTest.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace CodeTest.Rates
{
    public class UniRateService : IUniRateService
    {
        private static HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://api.unirateapi.com/api/")
        };

        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        private string _uniRateApiKey;
        private const string _uniRateApiKeyName = "UniRateApiKey";

        public UniRateService(
            IMemoryCache cache,
            IConfiguration configuration) 
        { 
            _memoryCache = cache;
            _configuration = configuration;

            if (_configuration == null) 
            {
                throw new ApplicationException("Issue grabbing configuration");
            }

            _uniRateApiKey = _configuration[_uniRateApiKeyName];

            if (string.IsNullOrEmpty(_uniRateApiKey))
            {
                throw new ApplicationException("Could not get api key");
            }
        }

        public async Task<decimal> GetRate(Currency buyCurrency, Currency sellCurrency)
        {
            decimal cachedValue;
            var expectedKey = $"{buyCurrency}>{sellCurrency}";

            if (!_memoryCache.TryGetValue(expectedKey, out cachedValue))
            {
                using HttpResponseMessage response = await _httpClient.GetAsync($"rates?api_key={}&from={CurrencyParser.ToString(sellCurrency)}");

                response.EnsureSuccessStatusCode();

                var rawResponse = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<RateObject>(rawResponse);

                cachedValue = 0;

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30));

                _memoryCache.Set(expectedKey, cachedValue, cacheEntryOptions);
            }

            return cachedValue;
        }
    }
}
