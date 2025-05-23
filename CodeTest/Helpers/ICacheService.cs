using CodeTest.Transfers.DTO;

namespace CodeTest.Helpers
{
    public interface ICacheService
    {
        decimal? GetFromCache(string expectedKey);
        QuoteResponse GetCachedQuote(string expectedKey);
        TransferResponse GetCachedTransfer(string expectedKey);
        void SetCache(string expectedKey, decimal cachedValue);
        void SetCacheQuote(string expectedKey, QuoteResponse cachedValue);
        void SetCachedTransfer(string expectedKey, TransferResponse cachedValue);
    }
}
