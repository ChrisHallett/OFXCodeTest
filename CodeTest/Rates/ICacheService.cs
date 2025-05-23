namespace CodeTest.Rates
{
    public interface ICacheService
    {
        decimal? GetFromCache(string expectedKey);
        void SetCache(string expectedKey, decimal cachedValue);
    }
}
