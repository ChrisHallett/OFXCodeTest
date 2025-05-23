
namespace CodeTest.Transfers
{
    public interface ITransferService
    {
        QuoteResponse GetQuote(Guid quoteId);
        Task<QuoteResponse> ProcessQuote(QuoteRequest request);
    }
}
