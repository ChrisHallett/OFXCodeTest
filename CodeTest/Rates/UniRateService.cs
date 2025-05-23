using CodeTest.Helpers;
using Newtonsoft.Json;

namespace CodeTest.Rates
{
    public class UniRateService : IUniRateService
    {
        private static HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://api.unirateapi.com/api/")
        };

        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;
        private string _uniRateApiKey;
        private const string _uniRateApiKeyName = "UniRateApiKey";

        public UniRateService(
            ICacheService cacheService,
            IConfiguration configuration) 
        { 
            _cacheService = cacheService;
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
            var expectedKey = $"{buyCurrency}>{sellCurrency}";

            var cachedValue = _cacheService.GetFromCache(expectedKey);

            if (cachedValue == null)
            {
                using HttpResponseMessage response = await _httpClient.GetAsync($"rates?api_key={_uniRateApiKey}&from={CurrencyParser.ToString(sellCurrency)}");

                response.EnsureSuccessStatusCode();

                var rawResponse = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<RateObject>(rawResponse);

                if(parsedResponse == null)
                {
                    throw new ApplicationException("No response found");
                }

                var foundRate = parsedResponse.Rates.FirstOrDefault(x => x.Key == CurrencyParser.ToString(buyCurrency));
                if(foundRate.Key == null)
                {
                    throw new ApplicationException("Could not find buy currency conversion rate");
                }

                cachedValue = foundRate.Value;
                _cacheService.SetCache(expectedKey, foundRate.Value);
            }

            return (decimal)cachedValue;
        }
    }
}
