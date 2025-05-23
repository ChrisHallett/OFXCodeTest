using Microsoft.Extensions.Caching.Memory;

namespace CodeTest.Rates
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheService(IMemoryCache memoryCache) 
        { 
            _memoryCache = memoryCache;
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
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30));
            _memoryCache.Set<decimal>(expectedKey, cachedValue, cacheEntryOptions);
        }
    }
}
