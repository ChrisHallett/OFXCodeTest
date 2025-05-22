namespace CodeTest.Rates
{
    public interface ICacheService
    {
        double? GetFromCache(string expectedKey);
        void SetCache(string expectedKey, double cachedValue);
    }
}
