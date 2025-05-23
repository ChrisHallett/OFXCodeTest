using CodeTest.Helpers;
using CodeTest.Rates;
using CodeTest.Transfers.DTO;

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

        public QuoteResponse GetQuote(Guid quoteId)
        {
            var quote = _cacheService.GetCachedQuote(quoteId.ToString());

            if (quote == null) 
            {
                throw new ApplicationException($"Quote with id {quoteId} not found");
            }

            return quote;
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

        public async Task<TransferResponse> CreateTransfer(TransferRequest request)
        {
            var rv = new TransferResponse();
            var existingQuote = _cacheService.GetCachedQuote(request.QuoteId.ToString());

            if (existingQuote == null)
            {
                throw new ApplicationException("Quote not found for transfer request");
            }

            rv.TransferId = Guid.NewGuid();
            rv.Status = Enum.GetName(TransferStatus.Processing);
            rv.TransferDetails = new TransferDetails()
            {
                Payer = request.Payer,
                Recipient = request.Recipient,
                QuoteId = existingQuote.QuoteId
            };

            rv.EstimatedDeliveryDate = DateTime.UtcNow.AddDays(1);

            _cacheService.SetCachedTransfer(rv.TransferId.ToString(), rv);

            return rv;
        }

        public TransferResponse GetTransfer(Guid transferId)
        {
            var transfer = _cacheService.GetCachedTransfer(transferId.ToString());

            if (transfer == null)
            {
                throw new ApplicationException($"Transfer with id {transferId} not found");
            }

            return transfer;
        }
    }
}
