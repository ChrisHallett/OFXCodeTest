
using CodeTest.Transfers.DTO;

namespace CodeTest.Transfers
{
    public interface ITransferService
    {
        QuoteResponse GetQuote(Guid quoteId);
        Task<QuoteResponse> ProcessQuote(QuoteRequest request);
        Task<TransferResponse> CreateTransfer(TransferRequest request);
        TransferResponse GetTransfer(Guid transferId);
    }
}
