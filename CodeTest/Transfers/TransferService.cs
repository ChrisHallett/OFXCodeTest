using CodeTest.Helpers;

namespace CodeTest.Transfers
{
    public class TransferService : ITransferService
    {
        public TransferService() { }

        public QuoteResponse ProcessQuote(QuoteRequest request)
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



            return rv;
        }
    }
}
