using CodeTest.Transfers;
using Microsoft.Extensions.Caching.Memory;

namespace CodeTest.Rates
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly int _apiRateCacheLifetime = 60;
        private readonly int _quoteCacheLifetime = 240;
        private readonly int _transferCacheLifetime = 240;

        public CacheService(IMemoryCache memoryCache) 
        { 
            _memoryCache = memoryCache;
        }

        public QuoteResponse GetCachedQuote(string expectedKey)
        {
            if (!_memoryCache.TryGetValue(expectedKey, out QuoteResponse cachedValue))
            {
                return null;
            }

            return cachedValue;
        }

        public TransferResponse GetCachedTransfer(string expectedKey)
        {
            if (!_memoryCache.TryGetValue(expectedKey, out TransferResponse cachedValue))
            {
                return null;
            }

            return cachedValue;
        }

        public decimal? GetFromCache(string expectedKey)
        {
            if (!_memoryCache.TryGetValue(expectedKey, out decimal cachedValue))
            {
                return null;
            }

            return cachedValue;
        }

        public void SetCache(string expectedKey, decimal cachedValue)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(_apiRateCacheLifetime));
            _memoryCache.Set<decimal>(expectedKey, cachedValue, cacheEntryOptions);
        }

        public void SetCachedTransfer(string expectedKey, TransferResponse cachedValue)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(_transferCacheLifetime));
            _memoryCache.Set<TransferResponse>(expectedKey, cachedValue, cacheEntryOptions);
        }

        public void SetCacheQuote(string expectedKey, QuoteResponse cachedValue)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(_quoteCacheLifetime));
            _memoryCache.Set<QuoteResponse>(expectedKey, cachedValue, cacheEntryOptions);
        }
    }
}
