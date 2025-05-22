using CodeTest.Helpers;

namespace CodeTest.Rates
{
    public interface IUniRateService
    {
        Task<decimal> GetRate(Currency buyCurrency, Currency sellCurrency);
    }
}
