using CodeTest.Helpers;
using CodeTest.Rates;

namespace CodeTest.Transfers
{
    public class TransferService : ITransferService
    {
        private readonly IUniRateService _rateService;
        private readonly ICacheService _cacheService;

        public TransferService(
            IUniRateService rateService,
            ICacheService cacheService) 
        {
            _rateService = rateService;
            _cacheService = cacheService;
        }

        public async Task<QuoteResponse> ProcessQuote(QuoteRequest request)
        {
            var rv = new QuoteResponse();

            var buyCurrency = CurrencyParser.ParseCurrency(request.BuyCurrency);
            var sellCurrency = CurrencyParser.ParseCurrency(request.SellCurrency);
            var amount = request.Amount;

            if(buyCurrency == sellCurrency)
            {
                throw new ApplicationException("Buy and sell currencies must not be the same");
            }

            if(amount <= 0)
            {
                throw new ApplicationException("Amount must be greater than zero");
            }

            var rate = await _rateService.GetRate(buyCurrency, sellCurrency);
            var inverseRate = await _rateService.GetRate(sellCurrency, buyCurrency);

            rv.OfxRate = rate;
            rv.InverseOfxRate = inverseRate;
            rv.QuoteId = Guid.NewGuid();
            rv.ConvertedAmount = decimal.Round(request.Amount * rate, 2);

            _cacheService.SetCacheQuote(rv.QuoteId.ToString(), rv);

            return rv;
        }
    }
}
