namespace CodeTest.Transfers
{
    public interface ITransferService
    {
        QuoteResponse ProcessQuote(QuoteRequest request);
    }
}
