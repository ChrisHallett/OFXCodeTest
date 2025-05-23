namespace CodeTest.Transfers
{
    public interface ITransferService
    {
        Task<QuoteResponse> ProcessQuote(QuoteRequest request);
    }
}
